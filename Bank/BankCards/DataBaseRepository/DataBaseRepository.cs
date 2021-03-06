using DataAbstraction;
using DataAbstraction.Repository;
using Microsoft.Extensions.Options;
using Npgsql;

namespace DataBaseRepository
{
    public class DataBaseRepository : IDataBaseRepository
    {
        private string _connectionString;

        public DataBaseRepository(IOptions<SelectRepositorySettings> repoSettings)
        {
            _connectionString = repoSettings.Value.DefaultConnection;
        }

        public async Task<bool> CheckCardIdExist(int id, CancellationTokenSource cts)
        {
            await using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                await using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText =
                        "SELECT " +
                            "\"Id\" " +
                        "FROM " +
                            "public.\"CardEntities\"" +
                        "WHERE " +
                            "\"Id\" = @Id;";

                    AddIdParamToCommand(cmd, id);

                    await using (var reader = await cmd.ExecuteReaderAsync(cts.Token))
                    {
                        while (await reader.ReadAsync(cts.Token))
                        {
                            return true;
                        }

                        return false;
                    }
                }
            }
        }

        public async Task<IEnumerable<CardEntity>> GetAllCards(CancellationTokenSource cts)
        {
            var result = new List<CardEntity>();

            await using (var conn = new NpgsqlConnection(_connectionString))
            {                
                conn.Open();

                await using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText =
                        "SELECT " +
                            "\"Id\", \"HolderName\", \"Number\", \"CVVCode\", \"Type\", \"System\", \"IsBlocked\" " +
                        "FROM " +
                            "public.\"CardEntities\";";
                    await using (var reader = await cmd.ExecuteReaderAsync(cts.Token))
                    {
                        while (await reader.ReadAsync(cts.Token))
                        {
                            CardEntity card = GetDataFromReaderToCard(reader);

                            result.Add(card);
                        }
                    }
                }
            }

            return result;
        }

        public async Task<CardEntity> GetCardById(int id, CancellationTokenSource cts)
        {
           await using (var conn = new NpgsqlConnection(_connectionString))
            {                
                conn.Open();

                await using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText =
                        "SELECT " +
                            "\"Id\", \"HolderName\", \"Number\", \"CVVCode\", \"Type\", \"System\", \"IsBlocked\" " +
                        "FROM " +
                            "public.\"CardEntities\"" +
                        "WHERE " +
                            "\"Id\" = @Id;";

                    AddIdParamToCommand(cmd, id);

                    await using (var reader = await cmd.ExecuteReaderAsync(cts.Token))
                    {
                        while (await reader.ReadAsync(cts.Token))
                        {
                            return GetDataFromReaderToCard(reader);
                        }

                        return null;
                    }
                }
            }
        }

        public async Task<CardEntity> GetCardByNumber(string number, CancellationTokenSource cts)
        {
            return await PrivateGetCardByNumber(number, cts);
        }

        private async Task<CardEntity> PrivateGetCardByNumber(string number, CancellationTokenSource cts)
        {
            await using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                await using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText =
                        "SELECT " +
                            "\"Id\", \"HolderName\", \"Number\", \"CVVCode\", \"Type\", \"System\", \"IsBlocked\" " +
                        "FROM " +
                            "public.\"CardEntities\"" +
                        "WHERE " +
                            "\"Number\" = @number;";

                    NpgsqlParameter param = new NpgsqlParameter();
                    param.ParameterName = "@number";
                    param.DbType = System.Data.DbType.String;
                    param.Value = number;
                    cmd.Parameters.Add(param);

                    await using (var reader = await cmd.ExecuteReaderAsync(cts.Token))
                    {
                        while (await reader.ReadAsync(cts.Token))
                        {
                            return GetDataFromReaderToCard(reader);
                        }

                        return null;
                    }
                }
            }
        }

        public async Task EditCardEntity(CardEntity cardEntity, CancellationTokenSource cts)
        {
            await PrivateSaveEditedCard(cardEntity, cts);
        }

        private async Task PrivateSaveEditedCard(CardEntity cardEntity, CancellationTokenSource cts)
        {
            await using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                await using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText =
                        "UPDATE  public.\"CardEntities\" SET " +
                            " (\"HolderName\", \"Number\", \"CVVCode\", \"Type\", \"System\", \"IsBlocked\") " +
                        " = " +
                            "(@HolderName, @Number, @CVVCode, @Type, @System, @IsBlocked) " +
                        " WHERE " +
                            "\"Id\" = @Id;";

                    AddAllParamsToCommand(cmd, cardEntity);
                    AddIdParamToCommand(cmd, cardEntity.Id);

                    await cmd.ExecuteNonQueryAsync(cts.Token);
                }
            }
        }

        public async Task<CardEntity> CreateNewCard(CardEntityToPost cardEntity, CancellationTokenSource cts)
        {
            CardEntity newCard = new CardEntity(cardEntity.HolderName, cardEntity.Number, cardEntity.CVVCode, cardEntity.Type, cardEntity.System, cardEntity.IsBlocked);

            await PrivateCreateNewCard(newCard, cts);

            return await PrivateGetCardByNumber(newCard.Number, cts);
        }

        public async Task<CardEntity> CreateNewCardAutoField(CardEntityToPostAutoField cardEntity, CancellationTokenSource cts)
        {
            CardEntity newCard = new CardEntity(cardEntity.HolderName, cardEntity.Type, cardEntity.System);

            await PrivateCreateNewCard(newCard, cts);

            return await PrivateGetCardByNumber(newCard.Number, cts);
        }

        public async Task DeleteCardEntity(int id, CancellationTokenSource cts)
        {
            await using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                await using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText =
                        "DELETE FROM " +
                            " public.\"CardEntities\" " +
                        " WHERE " +
                            " \"Id\" = @Id ;";


                    AddIdParamToCommand(cmd, id);

                    await cmd.ExecuteNonQueryAsync(cts.Token);
                }
            }
        }

        private CardEntity GetDataFromReaderToCard(NpgsqlDataReader reader)
        {
            CardEntity cardEntity = new CardEntity();

            cardEntity.Id = reader.GetInt32(0);
            cardEntity.HolderName = reader.GetString(1);
            cardEntity.Number = reader.GetString(2);
            cardEntity.CVVCode = reader.GetString(3);
            cardEntity.Type = (CardTypeEnum.CardType)reader.GetInt32(4);
            cardEntity.System = (CardSystemEnum.CardSystem)reader.GetInt32(5);
            cardEntity.IsBlocked = reader.GetBoolean(6);

            return cardEntity;
        }

        private async Task PrivateCreateNewCard(CardEntity newCard, CancellationTokenSource cts)
        {
            await using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                await using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText =
                        "INSERT INTO public.\"CardEntities\" " +
                            " (\"HolderName\", \"Number\", \"CVVCode\", \"Type\", \"System\", \"IsBlocked\") " +
                        "VALUES " +
                            "(@HolderName, @Number, @CVVCode, @Type, @System, @IsBlocked);";

                    AddAllParamsToCommand(cmd, newCard);

                    await cmd.ExecuteNonQueryAsync(cts.Token);
                }
            }
        }

        private void AddIdParamToCommand(NpgsqlCommand cmd, int id)
        {
            NpgsqlParameter paramId = new NpgsqlParameter();
            paramId.ParameterName = "@Id";
            paramId.DbType = System.Data.DbType.Int32;
            paramId.Value = id;
            cmd.Parameters.Add(paramId);
        }

        private void AddAllParamsToCommand(NpgsqlCommand cmd, CardEntity cardEntity)
        {
            NpgsqlParameter paramHolderName = new NpgsqlParameter();
            paramHolderName.ParameterName = "@HolderName";
            paramHolderName.DbType = System.Data.DbType.String;
            paramHolderName.Value = cardEntity.HolderName;
            cmd.Parameters.Add(paramHolderName);

            NpgsqlParameter paramNumber = new NpgsqlParameter();
            paramNumber.ParameterName = "@Number";
            paramNumber.DbType = System.Data.DbType.String;
            paramNumber.Value = cardEntity.Number;
            cmd.Parameters.Add(paramNumber);

            NpgsqlParameter paramCvv = new NpgsqlParameter();
            paramCvv.ParameterName = "@CVVCode";
            paramCvv.DbType = System.Data.DbType.String;
            paramCvv.Value = cardEntity.CVVCode;
            cmd.Parameters.Add(paramCvv);

            NpgsqlParameter paramType = new NpgsqlParameter();
            paramType.ParameterName = "@Type";
            paramType.DbType = System.Data.DbType.Int32;
            paramType.Value = (int)cardEntity.Type;
            cmd.Parameters.Add(paramType);

            NpgsqlParameter paramSystem = new NpgsqlParameter();
            paramSystem.ParameterName = "@System";
            paramSystem.DbType = System.Data.DbType.Int32;
            paramSystem.Value = (int)cardEntity.System;
            cmd.Parameters.Add(paramSystem);

            NpgsqlParameter paramBlocked = new NpgsqlParameter();
            paramBlocked.ParameterName = "@IsBlocked";
            paramBlocked.DbType = System.Data.DbType.Boolean;
            paramBlocked.Value = cardEntity.IsBlocked;
            cmd.Parameters.Add(paramBlocked);
        }
    }
}