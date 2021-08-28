using System;

namespace Application.Exceptions
{
    public class FeatureDisabledException : Exception
    {
        public FeatureDisabledException() : base("Feature Disabled")
        {

        }
    }
}
