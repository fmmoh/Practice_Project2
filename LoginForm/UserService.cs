using System.Data.SqlClient;


namespace LoginForm
{
    public class UserService
    {
        private string connectionString = @"Server=(localdb)\ProjectModels;Initial Catalog=DBFORM;Integrated Security=True;";


        public void Register(string username, string password)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username=@Username", conn);
            cmd.Parameters.AddWithValue("@Username", username);
            int count = (int)cmd.ExecuteScalar();

            if (count > 0)
            {
                Console.WriteLine("Registration failed! Username already exists.");
            }
            else
            {
                SqlCommand insertCmd = new SqlCommand("INSERT INTO Users (Username, Password, Status) VALUES (@Username, @Password, 'not available')", conn);
                insertCmd.Parameters.AddWithValue("@Username", username);
                insertCmd.Parameters.AddWithValue("@Password", password);
                insertCmd.ExecuteNonQuery();
                Console.WriteLine("Registration successful.");
            }
        }

        public bool Login(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username=@Username AND Password=@Password", conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                int count = (int)cmd.ExecuteScalar();

                if (count > 0)
                {
                    Console.WriteLine("Login successful.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Login failed! Invalid credentials.");
                    return false;
                }
            }
        }

        public void ChangeStatus(string username, string status)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Users SET Status=@Status WHERE Username=@Username", conn);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.ExecuteNonQuery();
                Console.WriteLine($"Status changed to {status}.");
            }
        }

        public void SearchUsers(string usernamePrefix)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Username, Status FROM Users WHERE Username LIKE @Prefix", conn);
                cmd.Parameters.AddWithValue("@Prefix", usernamePrefix + "%");

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Username"]} | status: {reader["Status"]}");
                    }
                }
            }
        }

        public void ChangePassword(string username, string oldPassword, string newPassword)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username=@Username AND Password=@OldPassword", conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@OldPassword", oldPassword);

                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    SqlCommand updateCmd = new SqlCommand("UPDATE Users SET Password=@NewPassword WHERE Username=@Username", conn);
                    updateCmd.Parameters.AddWithValue("@NewPassword", newPassword);
                    updateCmd.Parameters.AddWithValue("@Username", username);
                    updateCmd.ExecuteNonQuery();
                    Console.WriteLine("Password changed successfully.");
                }
                else
                {
                    Console.WriteLine("Password change failed! Old password is incorrect.");
                }
            }
        }
    }
}