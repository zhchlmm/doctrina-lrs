using Doctrina.Core.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Doctrina.xAPI.Models;
using Doctrina.Core.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Doctrina.Core.Repositories
{
    public class ActivityStateRepository : IActivityStateRepository
    {
        private readonly DoctrinaDbContext dbcontext;

        public ActivityStateRepository(DoctrinaDbContext context)
        {
            this.dbcontext = context;
        }

        public ActivityStateEntity GetState(string stateId, Uri activityId, AgentEntity agent, Guid? registration)
        {
            string strActivityId = activityId.ToString();

            //var sql = new Sql()
            //    .Select("*")
            //    .From<ActivityStateEntity>(this.context.SqlSyntax)
            //    .WhereAgent<ActivityStateEntity>(agent, this.context.SqlSyntax)
            //    .Where<ActivityStateEntity>(x => x.ActivityId == strActivityId, this.context.SqlSyntax)
            //    .Where<ActivityStateEntity>(x => x.StateId == stateId, this.context.SqlSyntax);

            var query = this.dbcontext.ActivityStates.WhereAgent(agent);

            if (registration.HasValue)
            {
                query.Where(x => x.RegistrationId == registration);
            }

            return query.SingleOrDefault();
        }

        public IEnumerable<ActivityStateEntity> GetStates(Uri activityId, AgentEntity agent, Guid? registration, DateTime? since)
        {
            string strActivityId = activityId.ToString();

            //var sql = new Sql()
            //    .Select("stateId", "updateDate")
            //    .From<ActivityStateEntity>(this.context.SqlSyntax)
            //    .WhereAgent<ActivityStateEntity>(agent, this.context.SqlSyntax)
            //    .Where<ActivityStateEntity>(x => x.ActivityId == strActivityId, this.context.SqlSyntax);

            var sql = this.dbcontext.ActivityStates.WhereAgent(agent)
                .Where(x => x.ActivityId == strActivityId);

            if (registration.HasValue)
            {
                sql.Where(x => x.RegistrationId == registration);
            }

            sql.OrderByDescending(x => x.Document.Timestamp);

            return sql;
        }

        /// <summary>
        /// Delete a single state document
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="agent"></param>
        /// <param name="stateId"></param>
        /// <param name="registration"></param>
        public void Delete(string stateId, Uri activityId, AgentEntity agent, Guid? registration)
        {
            string strActivityId = activityId.ToString();

            //var sql = new Sql()
            //    .From<ActivityStateEntity>(this.context.SqlSyntax)
            //    .WhereAgent<ActivityStateEntity>(agent, this.context.SqlSyntax);

            var sql = this.dbcontext.ActivityStates.WhereAgent(agent);
            
            // TODO: Implement Concurrency
            //if (!string.IsNullOrEmpty(etag))
            //{
            //    sql.Where<ActivityStateEntity>(x => x.ETag == etag, this.context.SqlSyntax);
            //}

            sql.Where(x => x.StateId == stateId)
                .Where(x => x.ActivityId == strActivityId);

            if (registration.HasValue)
            {
                sql.Where(x => x.RegistrationId == registration);
            }

            var entity = sql.FirstOrDefault();

            this.dbcontext.ActivityStates.Remove(entity);
            this.dbcontext.SaveChanges();
        }

        /// <summary>
        /// Deletes multiple state documents
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="agent"></param>
        /// <param name="registration"></param>
        public void Delete(Uri activityId, AgentEntity agent, Guid? registration)
        {
            string strActivityId = activityId.ToString();

            var sql = this.dbcontext.ActivityStates.WhereAgent(agent);

            sql.Where(x => x.ActivityId == strActivityId);

            if (registration.HasValue)
            {
                sql.Where(x => x.RegistrationId == registration);
            }

            this.dbcontext.ActivityStates.RemoveRange(sql);
            this.dbcontext.SaveChanges();
        }

        public ActivityStateEntity GetState(string stateId, Uri activityId, Agent agent, Guid? registration)
        {
            throw new NotImplementedException();
        }

        public void Save(ActivityStateEntity current)
        {
            dbcontext.ActivityStates.Add(current);
            dbcontext.Entry(current).State = EntityState.Modified;
        }

        public ActivityStateEntity Create(ActivityStateEntity activityState)
        {
            dbcontext.ActivityStates.Add(activityState);
            dbcontext.Entry(activityState).State = EntityState.Added;
            dbcontext.SaveChanges();

            return activityState;
        }

        public IEnumerable<DocumentEntity> GetStateDocuments(Uri activityId, AgentEntity agent, Guid? registration, DateTime? since)
        {
            string strActivityId = activityId.ToString();

            //var sql = new Sql()
            //    .Select("stateId", "updateDate")
            //    .From<ActivityStateEntity>(this.context.SqlSyntax)
            //    .WhereAgent<ActivityStateEntity>(agent, this.context.SqlSyntax)
            //    .Where<ActivityStateEntity>(x => x.ActivityId == strActivityId, this.context.SqlSyntax);

            var sql = this.dbcontext.ActivityStates.WhereAgent(agent)
                .Where(x => x.ActivityId == strActivityId);

            if (registration.HasValue)
            {
                sql.Where(x => x.RegistrationId == registration);
            }

            if (since.HasValue)
            {
                sql.Where(x => x.Document.Timestamp >= since.Value);
            }

            sql.OrderByDescending(x => x.Document.Timestamp);

            return sql.Select(x=> x.Document);
        }
    }
}
