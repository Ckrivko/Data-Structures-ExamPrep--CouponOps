namespace CouponOps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CouponOps.Models;
    using Interfaces;

    public class CouponOperations : ICouponOperations
    {
        private Dictionary<string, Website> websites;

        public CouponOperations()
        {
            this.websites = new Dictionary<string, Website>();
        }

        public void AddCoupon(Website website, Coupon coupon)
        {
            if (!this.Exist(website) || website.Coupons.Contains(coupon))
            {
                throw new ArgumentException();
            }

            website.Coupons.Add(coupon);
        }

        public bool Exist(Website website)
        {

            if (this.websites.ContainsKey(website.Domain))
            {
                return true;
            }
            return false;
        }

        public bool Exist(Coupon coupon)
        {
            foreach (var web in websites.Values)
            {
                if (web.Coupons.Contains(coupon))
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<Coupon> GetCouponsForWebsite(Website website)
        {
            if (!this.Exist(website))
            {

                throw new ArgumentException();
            }

            return website.Coupons;
        }

        public IEnumerable<Coupon> GetCouponsOrderedByValidityDescAndDiscountPercentageDesc()
        {
            var allCoupons = this.websites.Values
                .Select(x => x.Coupons)
                .ToArray();

            var resultCoupons = new List<Coupon>();

            foreach (var coupons in allCoupons)
            {
                resultCoupons.AddRange(coupons);
            }

            return resultCoupons.OrderByDescending(x => x.Validity).ThenByDescending(x => x.DiscountPercentage);
        }

        public IEnumerable<Website> GetSites()
        {
            return this.websites.Values.ToArray();
        }

        public IEnumerable<Website> GetWebsitesOrderedByUserCountAndCouponsCountDesc()
        {
          return this.websites.Values
                .OrderBy(x => x.UsersCount)
                .ThenByDescending(x => x.Coupons.Count)
                .ToArray();
                       
        }

        public void RegisterSite(Website website)
        {

            if (this.Exist(website))
            {
                throw new ArgumentException("We have website with this domain");
            }
            this.websites[website.Domain] = website;
        }

        public Coupon RemoveCoupon(string code)
        {

            foreach (var web in websites.Values)
            {
                Coupon couponResult = web.Coupons.FirstOrDefault(c => c.Code == code);

                if (couponResult != null)
                {
                    web.Coupons.Remove(couponResult);
                    return couponResult;
                }
            }

            throw new ArgumentException();

        }

        public Website RemoveWebsite(string domain)
        {
            if (!this.websites.ContainsKey(domain))
            {
                throw new ArgumentException($"We dont have this domain {domain}");
            }
            var websiteToRemove = this.websites[domain];

            this.websites.Remove(domain);
            return websiteToRemove;

        }

        public void UseCoupon(Website website, Coupon coupon)
        {
            if (!this.Exist(website))
            {
                throw new ArgumentException();

            }
            if (!website.Coupons.Contains(coupon))
            {
                throw new ArgumentException();
            }
            website.Coupons.Remove(coupon);


        }


    }
}
