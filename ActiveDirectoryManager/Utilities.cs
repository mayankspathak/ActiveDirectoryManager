using System;

namespace ActiveDirectoryManager.Utilities
{
    /// <summary>
    /// Common Utilities
    /// </summary>
    public sealed class Utils
    {
        /// <summary>
        /// Validates string length is greater than 3
        /// </summary>
        /// <param name="xName"></param>
        public static void validateLength(string xName)
        {
            if (xName.Trim().Replace("*", "").Length < 3)
                throw new ArgumentException("Minimum parameter length is 3.");
        }
    }
}
