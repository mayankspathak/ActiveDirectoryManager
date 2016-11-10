using ActiveDirectoryManager.DomainManager;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Xml.Linq;

namespace ActiveDirectoryManager.Computers
{
    /// <summary>
    /// Active Directory computers
    /// </summary>
    internal sealed class ActiveDirectoryComputers
    {
        /// <summary>
        /// Gets all the computers from Active Directory
        /// </summary>
        /// <returns></returns>
        internal static List<ComputerPrincipal> GetAll()
        {
            List<ComputerPrincipal> computerPrincipalList = new List<ComputerPrincipal>();
            using (PrincipalSearcher principalSearcher = new PrincipalSearcher((Principal)new ComputerPrincipal(new PrincipalContext(ContextType.Domain, DomainsManager.GetContextDomain()))))
            {
                foreach (ComputerPrincipal computerPrincipal in principalSearcher.FindAll())
                    computerPrincipalList.Add(computerPrincipal);
            }
            return computerPrincipalList;
        }

        /// <summary>
        /// Converts computer prinical to XElement
        /// </summary>
        /// <param name="computerPrincipal"></param>
        /// <returns></returns>
        internal static XElement ToXElement(ComputerPrincipal computerPrincipal)
        {
            return new XElement((XName)"ComputerPrincipal", new object[2]
            {
                (object) new XElement((XName) "Name", (object) computerPrincipal.Name),
                (object) new XElement((XName) "LastLogon", (object) computerPrincipal.LastLogon)
            });
        }
    }
}
