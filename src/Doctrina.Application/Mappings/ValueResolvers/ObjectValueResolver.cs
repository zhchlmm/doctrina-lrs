using AutoMapper;
using Doctrina.Domain.Entities;
using Doctrina.xAPI;
using System;

namespace Doctrina.Application.Mappings.ValueResolvers
{
    public class ObjectValueResolver : 
         IMemberValueResolver<object, object, Domain.Entities.IStatementObjectEntity, xAPI.IStatementObject>,
         IMemberValueResolver<object, object, xAPI.IStatementObject, Domain.Entities.StatementObjectEntity>
    {
        public xAPI.IStatementObject Resolve(object source, object destination, Domain.Entities.IStatementObjectEntity sourceMember, xAPI.IStatementObject destMember, ResolutionContext context)
        {
            try
            {
                if (sourceMember.ObjectType == EntityObjectType.Agent)
                {
                    return context.Mapper.Map<Agent>((AgentEntity)sourceMember);
                }
                else if (sourceMember.ObjectType == EntityObjectType.Group)
                {
                    return context.Mapper.Map<Group>((GroupEntity)sourceMember);
                }
                else if (sourceMember.ObjectType == EntityObjectType.Activity)
                {
                    return context.Mapper.Map<Activity>((ActivityEntity)sourceMember);
                }
                else if (sourceMember.ObjectType == EntityObjectType.SubStatement)
                {
                    return context.Mapper.Map<SubStatement>((SubStatementEntity)sourceMember);
                }
                else if (sourceMember.ObjectType == EntityObjectType.StatementRef)
                {
                    return context.Mapper.Map<StatementRef>((StatementRefEntity)sourceMember);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Domain.Entities.StatementObjectEntity Resolve(object source, object destination, xAPI.IStatementObject sourceMember, Domain.Entities.StatementObjectEntity destMember, ResolutionContext context)
        {
            try
            {
                var obj = new StatementObjectEntity();

                if (sourceMember.ObjectType == ObjectType.Agent)
                {
                    obj.ObjectType = EntityObjectType.Agent;
                    obj.Agent = context.Mapper.Map<AgentEntity>((xAPI.Agent)sourceMember);
                    return obj;
                }
                else if (sourceMember.ObjectType == ObjectType.Group)
                {
                    obj.ObjectType = EntityObjectType.Group;
                    obj.Agent = context.Mapper.Map<GroupEntity>((xAPI.Group)sourceMember);
                    return obj;
                }
                else if (sourceMember.ObjectType == ObjectType.Activity)
                {
                    obj.ObjectType = EntityObjectType.Activity;
                    obj.Activity = context.Mapper.Map<ActivityEntity>((xAPI.Activity)sourceMember);
                    return obj;
                }
                else if (sourceMember.ObjectType == ObjectType.SubStatement)
                {
                    obj.ObjectType = EntityObjectType.SubStatement;
                    obj.SubStatement = context.Mapper.Map<SubStatementEntity>((xAPI.SubStatement)sourceMember);
                    return obj;
                }
                else if (sourceMember.ObjectType == ObjectType.StatementRef)
                {
                    obj.ObjectType = EntityObjectType.StatementRef;
                    obj.StatementRef = context.Mapper.Map<StatementRefEntity>((xAPI.StatementRef)sourceMember);
                    return obj;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
