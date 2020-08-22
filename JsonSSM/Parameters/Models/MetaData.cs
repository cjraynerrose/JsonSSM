using System;
using System.Linq;

using Amazon;

using Newtonsoft.Json;

namespace JsonSSM.Models
{
    public class MetaData
    {
        public string[] Encrypt { get; }
        public RegionEndpoint Region { get; }

        [JsonConstructor]
        public MetaData(string[] encrypt, string region)
        {
            Region = ResolveRegion(region);

            if (encrypt != null)
            {
                Encrypt = EnsureFormatting(encrypt);
            }
        }

        private string[] EnsureFormatting(string[] strings)
        {
            for (int i = 0; i < strings.Length; i++)
            {
                strings[i] = EnsureFormatting(strings[i]);
            }

            return strings;
        }

        private RegionEndpoint ResolveRegion(string regionName)
        {
            if (string.IsNullOrWhiteSpace(regionName))
            {
                throw new ArgumentNullException(nameof(regionName));
            }
            var region = RegionEndpoint.GetBySystemName(regionName);
            if (region == default)
            {
                throw new ArgumentException($"Could not resolve region name '{regionName}' input to any AWS Region", nameof(regionName));
            }
            return region;
        }

        private string EnsureFormatting(string value)
        {
            if (!value.StartsWith('/'))
            {
                value = $"/{value}";
            }

            return value;
        }

        internal bool ShouldEncrypt(string prefix)
        {
            if (Encrypt == null || Encrypt.Length == 0)
            {
                return false;
            }

            prefix = EnsureFormatting(prefix);
            return Encrypt.Any(e =>
            {
                if (e.EndsWith('/')) // partial path
                {
                    return prefix.StartsWith(e);
                }
                else // else is a full path
                {
                    return prefix == e;
                }
            });
        }
    }
}