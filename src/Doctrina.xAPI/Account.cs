﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Doctrina.xAPI
{
    /// <summary>
    /// A user account on an existing system, such as a private system (LMS or intranet) or a public system (social networking site).
    /// </summary>
    public class Account : JsonModel<JObject>
    {

        public Account() { }
        public Account(string jsonString) : this(JObject.Parse(jsonString)) { }
        public Account(JObject jobj) : this(jobj, ApiVersion.GetLatest()) { }
        public Account(JObject jobj, ApiVersion version)
        {
            if(jobj["homePage"] != null)
            {
                HomePage = jobj.Value<Uri>("homePage");
            }

            if (jobj["name"] != null)
            {
                Name = jobj.Value<string>("name");
            }
        }

        /// <summary>
        /// The canonical home page for the system the account is on. This is based on FOAF's accountServiceHomePage.
        /// </summary>
        public Uri HomePage { get; set; }

        /// <summary>
        /// The unique id or name used to log in to this account. This is based on FOAF's accountName.
        /// </summary>
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            var account = obj as Account;
            return account != null &&
                   base.Equals(obj) &&
                   EqualityComparer<Uri>.Default.Equals(HomePage, account.HomePage) &&
                   Name == account.Name;
        }

        public override int GetHashCode()
        {
            var hashCode = 74222723;
            hashCode = hashCode * -1521134295 + EqualityComparer<Uri>.Default.GetHashCode(HomePage);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }

        public override JObject ToJToken(ApiVersion version, ResultFormat format)
        {
            var jobj = new JObject();
            if(HomePage != null)
            {
                jobj["homePage"] = HomePage.ToString();
            }

            if (!string.IsNullOrEmpty(Name))
            {
                jobj["name"] = Name;
            }

            return jobj;
        }

        public static bool operator ==(Account account1, Account account2)
        {
            return EqualityComparer<Account>.Default.Equals(account1, account2);
        }

        public static bool operator !=(Account account1, Account account2)
        {
            return !(account1 == account2);
        }
    }
}
