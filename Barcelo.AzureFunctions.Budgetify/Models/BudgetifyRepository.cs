﻿using Barcelo.AzureFunctions.Budgetify.Functions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Models
{
    public class BudgetifyRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _log;

        public BudgetifyRepository(IConfiguration configuration, ILogger log)
        {
            _configuration = configuration;
            _log = log;
        }
        public async Task<List<BudgetOptionsTable>> GetOptionsByBudgetId(int BudgetId)
        {
            try
            {
                _log.LogInformation($"Inicio de Repository GetOptionsByBudgetId con BudgetId:{BudgetId}");
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = $"SELECT * FROM [dbo].[BudgetOptions] WHERE BudgetId = {BudgetId}";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<BudgetOptionsTable> BudgetOptions = new List<BudgetOptionsTable>();

                            while (await reader.ReadAsync())
                            {
                                BudgetOptionsTable BudgetOption = new BudgetOptionsTable
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    BudgetId = reader.GetInt32(reader.GetOrdinal("BudgetId")),
                                    OptionDescription = reader.GetString(reader.GetOrdinal("OptionDescription"))
                                };

                                BudgetOptions.Add(BudgetOption);
                            }

                            return BudgetOptions;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation($"EXCEPTION Repository GetOptionsByBudgetId with budgetId:{BudgetId} - {ex}");
                return null;
            }

        }


        public async Task<List<BudgetTable>> GetBudgetsByAdminId(int adminId)
        {
            try
            {
                _log.LogInformation($"Inicio de Repository GetBudgetsByAdminId con adminId:{adminId}");
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = $"SELECT * FROM [dbo].[Budget] WHERE UserId = {adminId}";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<BudgetTable> budgets = new List<BudgetTable>();

                            while (await reader.ReadAsync())
                            {
                                BudgetTable budget = new BudgetTable
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    OrganizationId = reader.IsDBNull(reader.GetOrdinal("OrganizationId")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("OrganizationId")),
                                    Title = reader.GetString(reader.GetOrdinal("Title")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    From = reader.GetDateTime(reader.GetOrdinal("From")),
                                    To = reader.GetDateTime(reader.GetOrdinal("To")),
                                    ProposalFrom = reader.GetDateTime(reader.GetOrdinal("ProposalFrom")),
                                    ProposalTo = reader.GetDateTime(reader.GetOrdinal("ProposalTo")),
                                    ContractFile = reader.IsDBNull(reader.GetOrdinal("ContractFile")) ? null : reader.GetString(reader.GetOrdinal("ContractFile")),
                                    ContractName = reader.IsDBNull(reader.GetOrdinal("ContractName")) ? null : reader.GetString(reader.GetOrdinal("ContractName")),
                                    CreateDate = reader.GetDateTime(reader.GetOrdinal("CreateDate")),
                                    ModifyDate = reader.GetDateTime(reader.GetOrdinal("ModifyDate"))
                                };

                                budgets.Add(budget);
                            }

                            return budgets;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation($"EXCEPTION Repository GetBudgetsByAdminId con adminId:{adminId} - {ex}");
                return null;
            }

        }


        public async Task<List<UserBudgetTable>> GetBudgetsByUserId(int UserId)
        {
            try
            {
                _log.LogInformation($"Inicio de Repository GetBudgetsByUserId con UserId:{UserId}");
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = $"select [dbo].[Budget].*, [dbo].[BudgetOptions].OptionDescription, [dbo].[Voting].AutenticationToken " +
                        $" from [dbo].[Budget] " +
                        $"inner join [dbo].[Voting] on [dbo].[Budget].Id = [dbo].[Voting].BudgetId " +
                        $"left join [dbo].[BudgetOptions] on [dbo].[BudgetOptions].Id = [dbo].[Voting].BudgetOption " +
                        $"where [dbo].[Voting].UserId = {UserId}";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<UserBudgetTable> budgets = new List<UserBudgetTable>();

                            while (await reader.ReadAsync())
                            {
                                UserBudgetTable budget = new UserBudgetTable
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    OrganizationId = reader.IsDBNull(reader.GetOrdinal("OrganizationId")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("OrganizationId")),
                                    Title = reader.GetString(reader.GetOrdinal("Title")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    From = reader.GetDateTime(reader.GetOrdinal("From")),
                                    To = reader.GetDateTime(reader.GetOrdinal("To")),
                                    ProposalFrom = reader.GetDateTime(reader.GetOrdinal("ProposalFrom")),
                                    ProposalTo = reader.GetDateTime(reader.GetOrdinal("ProposalTo")),
                                    ContractFile = reader.IsDBNull(reader.GetOrdinal("ContractFile")) ? null : reader.GetString(reader.GetOrdinal("ContractFile")),
                                    ContractName = reader.IsDBNull(reader.GetOrdinal("ContractName")) ? null : reader.GetString(reader.GetOrdinal("ContractName")),
                                    CreateDate = reader.GetDateTime(reader.GetOrdinal("CreateDate")),
                                    ModifyDate = reader.GetDateTime(reader.GetOrdinal("ModifyDate")),
                                    BudgetOption = reader.IsDBNull(reader.GetOrdinal("OptionDescription")) ? string.Empty : reader.GetString(reader.GetOrdinal("OptionDescription")),
                                    AutenticationToken = reader.IsDBNull(reader.GetOrdinal("AutenticationToken")) ? string.Empty : reader.GetGuid(reader.GetOrdinal("AutenticationToken")).ToString()
                                };

                                budgets.Add(budget);
                            }

                            return budgets;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation($"EXCEPTION Repository GetBudgetsByUserId con UserId:{UserId} - {ex}");
                return null;
            }

        }


        public async Task<List<CompanyBudgetTable>> GetBudgetsByCompanyId(int CompanyId)
        {
            try
            {
                _log.LogInformation($"Inicio de Repository GetBudgetsByCompanyId con CompanyId:{CompanyId}");
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = $"SELECT [dbo].[Budget].*, [dbo].[Proposal].ProposalFile, [dbo].[Proposal].AutenticationToken " +
                        $"FROM [dbo].[Budget] " + 
                        $"LEFT JOIN [dbo].[Proposal] on [dbo].[Proposal].BudgetId = [dbo].[Budget].Id " + $"AND [dbo].[Proposal].UserId = {CompanyId} " +
                        $"WHERE GETDATE() BETWEEN [dbo].[Budget].ProposalFrom AND [dbo].[Budget].ProposalTo ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<CompanyBudgetTable> budgets = new List<CompanyBudgetTable>();

                            while (await reader.ReadAsync())
                            {
                                CompanyBudgetTable budget = new CompanyBudgetTable
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    CompanyId = CompanyId.ToString(),
                                    OrganizationId = reader.IsDBNull(reader.GetOrdinal("OrganizationId")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("OrganizationId")),
                                    Title = reader.GetString(reader.GetOrdinal("Title")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    From = reader.GetDateTime(reader.GetOrdinal("From")),
                                    To = reader.GetDateTime(reader.GetOrdinal("To")),
                                    ProposalFrom = reader.GetDateTime(reader.GetOrdinal("ProposalFrom")),
                                    ProposalTo = reader.GetDateTime(reader.GetOrdinal("ProposalTo")),
                                    ContractFile = reader.IsDBNull(reader.GetOrdinal("ContractFile")) ? null : reader.GetString(reader.GetOrdinal("ContractFile")),
                                    ContractName = reader.IsDBNull(reader.GetOrdinal("ContractName")) ? null : reader.GetString(reader.GetOrdinal("ContractName")),
                                    CreateDate = reader.GetDateTime(reader.GetOrdinal("CreateDate")),
                                    ModifyDate = reader.GetDateTime(reader.GetOrdinal("ModifyDate")),
                                    ProposalFile = reader.IsDBNull(reader.GetOrdinal("ProposalFile")) ? null : reader.GetString(reader.GetOrdinal("ProposalFile")),
                                    AutenticationToken = reader.IsDBNull(reader.GetOrdinal("AutenticationToken")) ? string.Empty : reader.GetGuid(reader.GetOrdinal("AutenticationToken")).ToString()
                                };

                                budgets.Add(budget);
                            }

                            return budgets;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation($"EXCEPTION Repository GetBudgetsByCompanyId con CompanyId:{CompanyId} - {ex}");
                return null;
            }

        }


        public async Task<bool> SaveBudget(CreateBudgetRequest request)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                        INSERT INTO [dbo].[Budget] 
                        ([UserId], [OrganizationId], [Title], [Description], [From], [To], 
                        [ProposalFrom], [ProposalTo], [ContractFile], [ContractName], [CreateDate], [ModifyDate])
                        OUTPUT INSERTED.Id
                        VALUES 
                        (@UserId, @OrganizationId, @Title, @Description, @From, @To, 
                        @ProposalFrom, @ProposalTo, @ContractFile, @ContractName, GETDATE(), GETDATE())";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", request.UserId);
                        command.Parameters.AddWithValue("@OrganizationId", request.OrganizationId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Title", request.Title);
                        command.Parameters.AddWithValue("@Description", request.Description);
                        command.Parameters.Add("@From", SqlDbType.DateTime).Value = request.From;
                        command.Parameters.Add("@To", SqlDbType.DateTime).Value = request.To;
                        command.Parameters.Add("@ProposalFrom", SqlDbType.DateTime).Value = request.ProposalFrom;
                        command.Parameters.Add("@ProposalTo", SqlDbType.DateTime).Value = request.ProposalTo;
                        command.Parameters.AddWithValue("@ContractFile", request.ContractFile ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ContractName", request.ContractName ?? (object)DBNull.Value);

                        int? insertedId = (int?)await command.ExecuteScalarAsync();
                        //Guardamos ahora las opciones
                        if (insertedId.HasValue)
                        {
                            int budgetId = insertedId.Value;
                            await SaveBudgetOptions(request.Options, budgetId);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _log.LogError($"Repository SaveBudget Error with Title: {request.Title}. {ex}");
                return false;
            }
        }

        public async Task<bool> SaveBudgetOptions(List<string> Options, int IdNewBudget)
        {
            try
            {
                if (Options.Any(option => !IsValidOption(option)))
                {
                    throw new ArgumentException("Inyección de SQL detectada");
                }

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string values = string.Join(",", Options.Select(option => $"({IdNewBudget}, '{option}')"));
                    _log.LogInformation($"Options for values in insert sql: {values}");

                    string query = $"insert into [BudgetOptions] (BudgetId, OptionDescription) values {values}";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _log.LogError($"Repository SaveBudgetOptions Error with budgetId: {IdNewBudget}. {ex}");
                return false;
            }
        }

        private bool IsValidOption(string option)
        {
            string[] forbiddenPatterns = { "'", ";", "--", "/*", "*/", "xp_", "exec", "sp_", "insert", "update", "delete", "select" };

            if (forbiddenPatterns.Any(pattern => option.Contains(pattern, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }
            return !string.IsNullOrEmpty(option) && option.Length <= 255;
        }


        public async Task<bool> DeleteBudgetOptions(List<string> KeysToDelete, int BudgetId)
        {
            try
            {
                if (KeysToDelete.Any(key => string.IsNullOrEmpty(key) || !IsValidOption(key)))
                {
                    throw new ArgumentException("Claves inválidas detectadas");
                }

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string keysString = string.Join(",", KeysToDelete.Select(key => Convert.ToInt32(key)));

                    string query = $"DELETE FROM [BudgetOptions] WHERE BudgetId = @BudgetId AND Id IN ({keysString})";
                    _log.LogInformation(query);
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BudgetId", BudgetId);
                        await command.ExecuteNonQueryAsync();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _log.LogError($"Repository DeleteBudgetOptions Error with budgetId: {BudgetId}. {ex}");
                return false;
            }
        }


        public async Task<bool> EditBudget(EditBudgetRequest request)
        {
            int rowsAffected = 0;
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                        UPDATE [Budget] SET 
                            [Title] = @Title, 
                            [Description] = @Description, 
                            [From] = @From, 
                            [To] = @To,
                            [ProposalFrom] = @ProposalFrom, 
                            [ProposalTo] = @ProposalTo,
                            [ContractFile] = CASE WHEN @ContractFile IS NOT NULL AND @ContractFile <> '' THEN @ContractFile ELSE [ContractFile] END
                        WHERE [Id] = @BudgetId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BudgetId", request.BudgetId);
                        command.Parameters.AddWithValue("@Title", request.Title);
                        command.Parameters.AddWithValue("@Description", request.Description);
                        command.Parameters.Add("@From", SqlDbType.DateTime).Value = request.From;
                        command.Parameters.Add("@To", SqlDbType.DateTime).Value = request.To;
                        command.Parameters.Add("@ProposalFrom", SqlDbType.DateTime).Value = request.From; //Por ahora la misma fecha
                        command.Parameters.Add("@ProposalTo", SqlDbType.DateTime).Value = request.To; //Por ahora la misma fecha
                        command.Parameters.AddWithValue("@ContractFile", request.ContractFile ?? string.Empty);

                        rowsAffected = command.ExecuteNonQuery();
                    } //Cerramos conexión porque ahora abriremos otra

                    //Actualizamos ahora las opciones
                    if (rowsAffected > 0)
                    {
                        int budgetId = request.BudgetId;
                        List<Dictionary<string, string>> options = request.Options;

                        //Los valores cuyo Key = 0, son nuevos valores por lo que se hace insert
                        List<string> newOptions = options
                            .Where(dict => dict.ContainsKey("0"))
                            .Select(dict => dict["0"])
                            .ToList();
                        await SaveBudgetOptions(newOptions, budgetId);

                        //Los valores cuyo Value finaliza en #ERASE# son borrados
                        List<string> removeOptions = options
                            .SelectMany(dict => dict.Where(kvp => kvp.Value.Contains("#ERASE#")).Select(kvp => kvp.Key))
                            .ToList();
                        await DeleteBudgetOptions(removeOptions, budgetId);
                    }
                    else
                    {
                        return false;
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                _log.LogError($"Repository EditBudget Error with budgetId: {request.BudgetId}. {ex}");
                return false;
            }
        }



        public async Task<BudgetTable> GetBudgetByIdAsync(int budgetId)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = $"SELECT * FROM [dbo].[Budget] WHERE Id = @BudgetId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BudgetId", budgetId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new BudgetTable
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    OrganizationId = reader.IsDBNull(reader.GetOrdinal("OrganizationId")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("OrganizationId")),
                                    Title = reader.GetString(reader.GetOrdinal("Title")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    From = reader.GetDateTime(reader.GetOrdinal("From")),
                                    To = reader.GetDateTime(reader.GetOrdinal("To")),
                                    ProposalFrom = reader.GetDateTime(reader.GetOrdinal("ProposalFrom")),
                                    ProposalTo = reader.GetDateTime(reader.GetOrdinal("ProposalTo")),
                                    ContractFile = reader.IsDBNull(reader.GetOrdinal("ContractFile")) ? null : reader.GetString(reader.GetOrdinal("ContractFile")),
                                    ContractName = reader.IsDBNull(reader.GetOrdinal("ContractName")) ? null : reader.GetString(reader.GetOrdinal("ContractName")),
                                    CreateDate = reader.GetDateTime(reader.GetOrdinal("CreateDate")),
                                    ModifyDate = reader.GetDateTime(reader.GetOrdinal("ModifyDate"))
                                };
                            }
                            else
                            {
                                return null; // Si no se encuentra el registro con el Id proporcionado
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogError($"Repository GetBudgetByIdAsync Error with budgetId: {budgetId}. {ex}");
                return null;
            }
        }


        public async Task<bool> SaveUserAsync(CreateUserRequest request)
        {
            try
            {
                _log.LogInformation($"Inicio de Repository SaveUserAsync con {request.Email}");
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                _log.LogInformation($"ConnectionString: {connectionString}");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                INSERT INTO [dbo].[User] 
                ([Type], [Name], [DNI], [Surnames], [Email], [Login], [Token])
                VALUES 
                ((select top 1 Id from [dbo].[UserType] where UPPER([type]) = UPPER(@Type)), 
                @Name, @DNI, @Surnames, @Email, @Login, @Token)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Type", request.Type);
                        command.Parameters.AddWithValue("@Name", request.Name);
                        command.Parameters.AddWithValue("@DNI", request.DNI);
                        command.Parameters.AddWithValue("@Surnames", request.Surnames ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Email", request.Email);
                        command.Parameters.AddWithValue("@Login", request.Login ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Token", request.Token ?? (object)DBNull.Value);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                _log.LogInformation($"Fin de Repository SaveUserAsync con {request.Email} OK");
                return true; // La inserción fue exitosa
            }
            catch (Exception ex)
            {
                _log.LogError($"Fin de Repository SaveUserAsync con {request.Email} KO. {ex}");
                return false; // La inserción falló
            }
        }


        public async Task<bool> SaveSessionId(string login, string token)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                        insert into [dbo].[Sessions] (UserId, Token) values 
                        ((select top 1 Id from [dbo].[User] where [login] = @login order by Id desc), @token)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@login", login);
                        command.Parameters.AddWithValue("@token", token);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                _log.LogInformation($"Fin de Repository SaveSessionId OK");
                return true;
            }
            catch (Exception ex)
            {
                _log.LogError($"Fin de Repository SaveSessionId con KO. {ex}");
                return false;
            }
        }


        public async Task<string> SessionByLogin(string login)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = $"select TOP 1 Token from [dbo].[Sessions] where UserId = (select top 1 id from [User] where [login] = @login) and ExpirationDate > GETDATE() order by Id desc";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@login", login);

                        object tokenObj = await command.ExecuteScalarAsync();

                        if (tokenObj != null && tokenObj != DBNull.Value)
                        {
                            string sessionId = Convert.ToString(tokenObj);
                            return sessionId;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogError($"Repository SessionId error with {login} KO. {ex}");
                return null;
            }
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest userpwd)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string login = userpwd.username.ToString();
                    string pwd = userpwd.password.ToString();

                    string query = "SELECT Id, Name, Email, Login, type " +
                                   "FROM [dbo].[User] " +
                                   "WHERE Login = @login AND Token = @pwd";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@login", login);
                        command.Parameters.AddWithValue("@pwd", pwd);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                int userId = reader.GetInt32(reader.GetOrdinal("Id"));
                                string userName = reader.GetString(reader.GetOrdinal("Name"));
                                string userEmail = reader.GetString(reader.GetOrdinal("Email"));
                                string userLogin = reader.GetString(reader.GetOrdinal("Login"));
                                int role = reader.GetInt32(reader.GetOrdinal("type"));

                                Guid newGuid = Guid.NewGuid();
                                string sessionId = $"{newGuid}";

                                //Guardamos la sesión cada vez que hace login
                                string token = $"{role}{sessionId}";
                                await this.SaveSessionId(login, token);

                                var userdata = new
                                {
                                    Id = userId,
                                    Name = userName,
                                    Email = userEmail,
                                    Login = userLogin,
                                    Token = token
                                };
                                string jsonUserData = JsonConvert.SerializeObject(userdata);

                                // Crear y devolver la respuesta
                                var response = new LoginResponse
                                {
                                    sessionId = sessionId,
                                    role = role,
                                    userData = jsonUserData
                                };



                                return response;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogError($"Repository Login error with {userpwd.username} KO. {ex}");
                return null;
            }
        }

        public async Task<string> UserBudgetVoting(UserBudgetVotingRequest req)
        {
            int rowsAffected = 0;
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string newToken = Guid.NewGuid().ToString();
                    string query = "UPDATE [Voting] SET [BudgetOption] = @OptionId, [AutenticationToken] = @NewToken " +
                         "WHERE [BudgetId] = @BudgetId AND [UserId] = @UserId " +
                         "AND EXISTS (SELECT 1 FROM BudgetOptions WHERE [BudgetId] = @BudgetId AND Id = @OptionId)";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BudgetId", req.BudgetId);
                        command.Parameters.AddWithValue("@UserId", int.Parse(req.UserId));
                        command.Parameters.AddWithValue("@OptionId", req.OptionId);
                        command.Parameters.AddWithValue("@NewToken", newToken);

                        rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return newToken;
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                }
                _log.LogInformation($"Fin de Repository SaveSessionId OK");
                return string.Empty;
            }
            catch (Exception ex)
            {
                _log.LogError($"Fin de Repository SaveSessionId con KO. {ex}");
                return string.Empty;
            }
        }


        public async Task<int> MapUsersToBudget(MapUsersToBudgetRequest req)
        {
            int rowsAffected = 0;
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Crear la tabla temporal
                    var createTableQuery = "CREATE TABLE #TempDNI (DNI NVARCHAR(255) NOT NULL)";
                    using (SqlCommand createTableCommand = new SqlCommand(createTableQuery, connection))
                    {
                        createTableCommand.ExecuteNonQuery();
                    }

                    // Insertar datos en la tabla temporal
                    var insertDataQuery = "INSERT INTO #TempDNI (DNI) VALUES (@DNI)";
                    foreach (var dni in req.UserList)
                    {
                        using (SqlCommand insertDataCommand = new SqlCommand(insertDataQuery, connection))
                        {
                            insertDataCommand.Parameters.AddWithValue("@DNI", dni);
                            insertDataCommand.ExecuteNonQuery();
                        }
                    }

                    // Utilizar la tabla temporal en la consulta principal
                    string insertQuery = @"
                        INSERT INTO [dbo].[Voting] (BudgetId, UserId, AutenticationToken)
                        SELECT @BudgetId, U.Id, NEWID()
                        FROM [dbo].[User] U
                        INNER JOIN #TempDNI D ON U.DNI = D.DNI
                        WHERE U.type = 3
                            AND NOT EXISTS (
                                        SELECT 1
                                        FROM [dbo].[Voting] V
                                        WHERE V.BudgetId = @BudgetId
                                        AND V.UserId = U.Id
                                    )";

                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@BudgetId", req.BudgetId);
                        rowsAffected = insertCommand.ExecuteNonQuery();
                    }

                    // Eliminar la tabla temporal
                    var dropTableQuery = "DROP TABLE #TempDNI";
                    using (SqlCommand dropTableCommand = new SqlCommand(dropTableQuery, connection))
                    {
                        dropTableCommand.ExecuteNonQuery();
                    }
                }
                _log.LogInformation($"Fin de Repository MapUsersToBudget OK");
                return rowsAffected;
            }
            catch (Exception ex)
            {
                _log.LogError($"Fin de Repository MapUsersToBudget con KO. {ex}");
                return 0;
            }
        }


        public async Task<string> UpdateProposal(UpdateProposalRequest request)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                string newToken = Guid.NewGuid().ToString();
                int inserted = 0;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                        INSERT INTO [dbo].[Proposal] 
                        ([UserId], [BudgetId], [ProposalFile], [AutenticationToken])
                        OUTPUT INSERTED.Id
                        VALUES 
                        (@UserId, @BudgetId, @ProposalFile, @AutenticationToken)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", int.Parse(request.CompanyId));
                        command.Parameters.AddWithValue("@BudgetId", request.BudgetId);
                        command.Parameters.AddWithValue("@ProposalFile", request.ProposalContract);
                        command.Parameters.AddWithValue("@AutenticationToken", newToken);                        

                        int? insertedId = (int?)await command.ExecuteScalarAsync();
                        if (insertedId.HasValue)
                        {
                            inserted = insertedId.Value;
                        }
                    }
                }
                if(inserted > 0)
                {
                    return newToken;
                }
                else
                {
                    return string.Empty;
                }
                
            }
            catch (Exception ex)
            {
                string pair = $"CompanyId: {request.CompanyId}, BudgetId: {request.BudgetId}";
                _log.LogError($"Repository UpdateProposal {pair}. Error: {ex}");
                return string.Empty;
            }
        }



    }

}
