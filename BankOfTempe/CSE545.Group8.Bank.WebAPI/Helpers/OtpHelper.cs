using CSE545.Group8.Bank.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace CSE545.Group8.Bank.WebAPI.Helpers
{
    public static class OtpHelper
    {
        private const string OTP_HEADER = "X-OTP";

        public static bool HasValidTotp(this HttpRequestMessage request, string key)
        {
            if (request.Headers.Contains(OTP_HEADER))
            {
                string otp = request.Headers.GetValues(OTP_HEADER).First();

                // We need to check the passcode against the past, current, and future passcodes

                if (!string.IsNullOrWhiteSpace(otp))
                {
                    var keys = TimeSensitivePassCode.GetListOfOTPs(key);
                    if (keys != null && keys.Contains(otp))
                    {
                        return true;
                    }
                }

            }
            return false;
        }
    }
}