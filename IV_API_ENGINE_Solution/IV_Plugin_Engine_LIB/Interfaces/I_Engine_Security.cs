using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVolt.Apps.PluginEngine.Interfaces
{
	public interface I_Engine_Security
	{
		string EncryptText(string text);
		string DecryptText(string text);
		string HashText(string text);

		bool UseDomainForSecurity(bool useDomain);
		bool AddDomain(string domainName, string adminUsername = "", string adminPassword = "");
		bool AddUser(string username, string password, string adminUsername = "", string adminPassword = "");
		bool RemoveUser(string username, string adminUsername = "", string adminPassword = "");
		bool ValidateUser(string username, string password, string adminUsername = "", string adminPassword = "");
		bool ModifyUser(string username, string newPassword, string adminUsername = "", string adminPassword = "");
		bool AddMenuToUser(string username, string menuID, string adminUsername = "", string adminPassword = "");
		bool RemoveMenuFromUser(string username, string menuID, string adminUsername="", string adminPassword="");
		bool MakeUserAdmin(string username, string adminUsername="", string adminPassword = "");
		bool RemoveAdmin(string username, string adminUsername = "", string adminPassword = "");
	}
}
