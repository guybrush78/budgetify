using Barcelo.AzureFunctions.Budgetify.Functions;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Models
{
    public class BudgetifyRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<BudgetifyRepository> _log;

        public BudgetifyRepository(IConfiguration configuration, ILogger<BudgetifyRepository> log)
        {
            _configuration = configuration;
            _log = log;
        }

        public async Task<List<BudgetTable>> GetBudgetsByAdminId(int adminId)
        {
            try
            {
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
                                    ContractFile = reader.IsDBNull(reader.GetOrdinal("ContractFile")) ? null : (byte[])reader["ContractFile"],
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
                VALUES 
                (@UserId, @OrganizationId, @Title, @Description, @From, @To, 
                @ProposalFrom, @ProposalTo, @ContractFile, @ContractName, GETDATE(), GETDATE())";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", request.UserId);
                        command.Parameters.AddWithValue("@OrganizationId", request.OrganizationId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Title", request.Title);
                        command.Parameters.AddWithValue("@Description", request.Description);
                        command.Parameters.AddWithValue("@From", request.From);
                        command.Parameters.AddWithValue("@To", request.To);
                        command.Parameters.AddWithValue("@ProposalFrom", request.ProposalFrom);
                        command.Parameters.AddWithValue("@ProposalTo", request.ProposalTo);
                        command.Parameters.AddWithValue("@ContractFile", request.ContractFile ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ContractName", request.ContractName ?? (object)DBNull.Value);

                        await command.ExecuteReaderAsync();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
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
                                    ContractFile = reader.IsDBNull(reader.GetOrdinal("ContractFile")) ? null : (byte[])reader["ContractFile"],
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
            catch (Exception)
            {
                // Manejar cualquier excepción aquí
                return null;
            }
        }


        public async Task<bool> SaveUserAsync(CreateUserRequest request)
        {
            try
            {
                _log.LogInformation($"Inicio de Repository SaveUserAsync con {request.Email}");
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                INSERT INTO [dbo].[User] 
                ([Type], [Name], [DNI], [Surnames], [Email], [Login], [Token])
                VALUES 
                (@Type, @Name, @DNI, @Surnames, @Email, @Login, @Token)";

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


    }
}
