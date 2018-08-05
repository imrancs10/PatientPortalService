using AsyncPoco;
using SWN.MobileService.Api.Data.Entities;
using System.Threading.Tasks;

namespace SWN.MobileService.Api.Services
{
    public class GetWordBackService : IGetWordBackService
    {
        private readonly IDatabase _swn402Db;
        public GetWordBackService(IDatabase swn402Db)
        {
            _swn402Db = swn402Db;
        }

        public async Task<int> UpdateResponse(MessageDetail message, int getWordBackOptionId)
        {
            var sqlQuery = Sql.Builder
                .Append($@" IF (NOT EXISTS(
                  SELECT *  FROM recipient_message_wordback WITH (NOLOCK)    
                            WHERE message_id       = {message.MessageTransactionId}    
                                  AND recipient_id     = {message.RecipientId}    
                                  AND contact_point_id = {message.ContactPointId}))
                     BEGIN 
                          INSERT INTO recipient_message_wordback 
                                (recipient_id, wordback_id, contact_point_id, message_id )  
                                VALUES({message.RecipientId}, {getWordBackOptionId}
                                , {message.ContactPointId}, {message.MessageTransactionId} )
                     END 
                     ELSE 
                     BEGIN 
                         UPDATE recipient_message_wordback  
                         SET wordback_id = {getWordBackOptionId}
                         WHERE message_id = {message.MessageTransactionId}  
                         AND recipient_id = {message.RecipientId}
                         AND contact_point_id = {message.ContactPointId}  
                     END 
                    ");

            return await _swn402Db.ExecuteAsync(sqlQuery);
        }
    }
}
