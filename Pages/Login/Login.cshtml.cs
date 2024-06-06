using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace Form.Pages
{
    public class LoginModel : PageModel
    {
        private readonly string _connectionString = "Server=localhost;Database=Login;Uid=root;Pwd=;";
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(ILogger<LoginModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost(string userName, string Password)
        {
            // Kết nối đến cơ sở dữ liệu
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Tạo truy vấn SQL để kiểm tra tài khoản
                string query = "SELECT * FROM user WHERE userName = @userName AND password = @Password";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@userName", userName);
                command.Parameters.AddWithValue("@password", Password);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Tài khoản hợp lệ, thực hiện xử lý tại đây
                        return RedirectToPage("/MasterLayout");
                    }
                    else
                    {
                        // Tài khoản không hợp lệ, trả về lại trang đăng nhập
                        return Page();
                    }
                }
            }
        }
    }
}