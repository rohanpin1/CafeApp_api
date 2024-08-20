using CafeApp_api.DTO;
using CafeApp_api.ResponseRequests;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CafeApp_api.Service
{
    public class CafeService : ICafeService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly ITokenService _tokenService;
        public CafeService(IConfiguration configuration, ITokenService tokenService)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("myconn");
            _tokenService = tokenService;

        }
        public async Task<List<Users>> GetAllUsers()
        {
            try
            {
                var users = new List<Users>();
                string connection = _configuration.GetConnectionString("myconn");
                var openCon = new SqlConnection(connection);

                var cmd = new SqlCommand()
                {
                    CommandText = "select * from users",
                    CommandType = System.Data.CommandType.Text,
                    Connection = openCon
                };
                openCon.Open();
                var row = cmd.ExecuteReader();

                while (row.Read())
                {
                    Users user = new Users();
                    user.Name = (string)row["name"];
                    user.Phone = (string)row["phone"];
                    user.Email = (string)row["email"];
                    user.City = (string)row["city"];
                    user.Id = (int)row["id"];
                    users.Add(user);
                }

                openCon.Close();
                return users;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteRow(int id)
        {
            try
            {
                var openCon = new SqlConnection(_connectionString);
                var cmd = new SqlCommand()
                {
                    CommandText = $"delete from users where id = {id}",
                    CommandType = System.Data.CommandType.Text,
                    Connection = openCon
                };
                openCon.Open();
                var row = cmd.ExecuteNonQuery();
                openCon.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Users> GetUser(int id)
        {
            try
            {
                Users user = new Users();
                var openCon = new SqlConnection(_connectionString);
                var cmd = new SqlCommand()
                {
                    CommandText = $"select * from users where id = {id}",
                    CommandType = System.Data.CommandType.Text,
                    Connection = openCon
                };
                openCon.Open();
                var row = cmd.ExecuteReader();
                while (row.Read())
                {
                    user.Name = (string)row["name"];
                    user.Phone = (string)row["phone"];
                    user.Email = (string)row["email"];
                    user.City = (string)row["city"];
                    user.Id = (int)row["id"];
                }
                openCon.Close();
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateUser(Users user)
        {
            try
            {
                //Users user = JsonConvert.DeserializeObject<Users>(data);
                var openCon = new SqlConnection(_connectionString);
                var cmd = new SqlCommand()
                {
                    CommandText = $"update users set name = '{user.Name}',phone = '{user.Phone}',email = '{user.Email}',city='{user.City}' where id = {user.Id}",
                    CommandType = System.Data.CommandType.Text,
                    Connection = openCon
                };
                openCon.Open();
                var row = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Users> GetUserByPhoneNumber(string phoneNumber)
        {
            try
            {
                Users user = new();
                var openCon = new SqlConnection(_connectionString);
                var cmd = new SqlCommand()
                {
                    CommandText = $"select * from Users where Phone = '{phoneNumber}'",
                    CommandType = System.Data.CommandType.Text,
                    Connection = openCon
                };
                openCon.Open();
                var row = cmd.ExecuteReader();

                while (row.Read())
                {
                    user.Name = (string)row["name"];
                    user.Phone = (string)row["phone"];
                    user.Email = (string)row["email"];
                    user.City = (string)row["city"];
                    user.Id = (int)row["id"];
                }
                return user;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<bool> RegisterLoginUser(AuthenticateUser user)
        {
            try
            {
                //var isUserExist =                
                var openCon = new SqlConnection(_connectionString);
                int Is2FAEnabled = user.Is2FAEnabled == true ? 1 : 0;
                var cmd = new SqlCommand()
                {
                    CommandText = $"select COUNT(1) from AuthenticateUsers where Username = '{user.Email}'",
                    CommandType = System.Data.CommandType.Text,
                    Connection = openCon
                };

                openCon.Open();
                var isExist = (int)cmd.ExecuteScalar();

                if (isExist == 1)
                {
                    return false;
                }
                else
                {
                    string twofacode = Is2FAEnabled == 1 ? Random2FAString() : string.Empty;

                    cmd = new SqlCommand()
                    {
                        CommandText = $"insert into AuthenticateUsers(Username, Password, IsTwoFA, TwoFACode) values ('{user.Email}','{user.Password}',{Is2FAEnabled},'{twofacode}')",
                        CommandType = System.Data.CommandType.Text,
                        Connection = openCon
                    };

                    cmd.ExecuteNonQuery();

                    if (user.Is2FAEnabled)
                    {
                        Send2FAString(user.Email, twofacode);
                    }
                    openCon.Close();
                    return true;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private string Random2FAString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            StringBuilder result = new StringBuilder(6);

            for (int i = 0; i < 6; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }


        private void Send2FAString(string email, string TwoFAString)
        {
            try
            {
                SmtpClient client = new("smtp-relay.brevo.com", 587);
                client.Credentials = new NetworkCredential("rohankumawat.pinkcity@gmail.com", "fvD6nHztdKw5qRhy");
                client.EnableSsl = true;


                MailAddress from = new MailAddress("rohankumawat.pinkcity@gmail.com",
               "Registration Successfull!",
            System.Text.Encoding.UTF8);

                MailAddress to = new(email);

                MailMessage message = new MailMessage(from, to);

                message.Body = TwoFAString;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Subject = "Two Factor Authentication Code";
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                client.Send(message);
                message.Dispose();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<LoginResponse> Login(AuthenticateUser user)
        {
            try
            {
                var openCon = new SqlConnection(_connectionString);
                int Is2FAEnabled = user.Is2FAEnabled == true ? 1 : 0;
                var cmd = new SqlCommand()
                {
                    CommandText = $"select * from AuthenticateUsers where Username = '{user.Email}' and Password = '{user.Password}'",
                    CommandType = System.Data.CommandType.Text,
                    Connection = openCon
                };

                openCon.Open();
                var isExist = cmd.ExecuteReader();
                LoginResponse res = new();
                string twofacode,email = string.Empty;
                bool isTFA = false;
                while (isExist.Read())
                {
                    twofacode = (string)isExist["twofacode"];
                    isTFA = (bool)isExist["istwofa"];      
                    email = (string)isExist["username"];
                }
                if (!string.IsNullOrEmpty(email))
                {
                    string token = await _tokenService.GetToken(user);

                    res.Is2FAEnabled = isTFA;
                    res.FlagCode = 1;
                    res.Token = token;
                    return res;
                }
                else
                {
                    res.Token = null;
                    res.FlagCode = 0;
                    res.Is2FAEnabled = false;
                    return res;
                }
                
            }
            catch (Exception)
            {
                return new() { FlagCode = 0, Token = null ,Is2FAEnabled = false};
                throw;
            }
        }
    }
}