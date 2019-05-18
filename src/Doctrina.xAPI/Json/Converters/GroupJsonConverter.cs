using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Doctrina.xAPI.Json.Converters
{
    public class GroupJsonConverter : ApiJsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Group) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            JObject jobj = JObject.Load(reader);

            // Default to agent
            ObjectType objType = ObjectType.Agent;
            var jObjectType = jobj["objectType"];
            if (jObjectType != null)
            {
                objType = jObjectType.Value<string>();
                //if (!Enum.TryParse(strObjectType, out objectType))
                //    throw new JsonSerializationException($"'{strObjectType}' is not valid. Path: '{jObjectType.Path}'");

                //if(!Enum.IsDefined(typeof(ObjectType), objectType))
                //    throw new JsonSerializationException($"'{strObjectType}' is not valid. Path: '{jObjectType.Path}'");
            }


            //var ifiNames = new string[] { "mbox", "mbox_sha1sum", "openid", "account" };
            //int ifiCount = jobj.Properties().Where(x => ifiNames.Contains(x.Name)).Count();
            //var isAnonymous = ifiCount == 0;

            //if (objectType == ObjectType.Agent)
            //{
            //    if (isAnonymous)
            //    {
            //        throw new JsonSerializationException($"An Agent MUST be identified by one (1) of the four types of Inverse Functional Identifiers.");
            //    } else if (ifiCount > 1)
            //    {
            //        throw new JsonSerializationException($"An Agent MUST NOT include more than one (1) Inverse Functional Identifier.");
            //    }
            //} else if (objectType == ObjectType.Group) {
            //    if (isAnonymous)
            //    {
            //        JToken member = jobj["member"];
            //        if (!member.HasValues)
            //        {
            //            throw new JsonSerializationException($"Anonymous Group missing member. Path: {member.Path}");
            //        }
            //    }
            //}

            // Validate Agent name as string
            IsString(jobj["name"]);

            //if (reader.Path == "authority")
            //{
            //    // Authority MUST be an Agent .. 
            //    if (objectType == ObjectType.Agent)
            //        return jobj.ToObject<Agent>();

            //    // .. except in 3-legged OAuth, where it MUST be a Group with two Agents. 
            //    if (objectType == ObjectType.Group)
            //    {
            //        var group = new Group();
            //        serializer.Populate(jobj.CreateReader(), group);

            //        // The two Agents represent an application and user together.
            //        if (group.Member.Count() != 2)
            //            throw new JsonSerializationException("Group must contains exactly two Agents.");

            //        if (group.IsIdentified())
            //            throw new JsonSerializationException("Identified group is not allowed");

            //        return group;
            //    }

            //    throw new JsonSerializationException($"'{objectType}' is not allowed as authority.");
            //}

            IStatementObject target = objType.CreateInstance();

            serializer.Populate(jobj.CreateReader(), target);
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;
    }
}
