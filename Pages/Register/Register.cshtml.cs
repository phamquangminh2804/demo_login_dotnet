using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Form.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly string _connectionString = "Server=localhost;Database=Login;Uid=root;Pwd=;";

        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string FullName { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Kiểm tra xem tên người dùng đã tồn tại trong cơ sở dữ liệu chưa
                string checkUserQuery = "SELECT COUNT(*) FROM user WHERE username = @userName";
                MySqlCommand checkUserCommand = new MySqlCommand(checkUserQuery, connection);
                checkUserCommand.Parameters.AddWithValue("@userName", UserName);

                int userCount = Convert.ToInt32(checkUserCommand.ExecuteScalar());

                if (userCount > 0)
                {
                    ModelState.AddModelError(string.Empty, "Tên người dùng đã tồn tại.");
                    ViewData["ErrorMessage"] = "Tên người dùng đã tồn tại.";

                    return Page();
                }
                else
                {
                    // Thêm tên người dùng và mật khẩu vào cơ sở dữ liệu
                    string insertUserQuery = "INSERT INTO user (FullName, userName, Password) VALUES (@FullName, @userName, @Password)";
                    MySqlCommand insertUserCommand = new MySqlCommand(insertUserQuery, connection);
                    insertUserCommand.Parameters.AddWithValue("@userName", UserName);
                    insertUserCommand.Parameters.AddWithValue("@FullName", FullName);
                    insertUserCommand.Parameters.AddWithValue("@Password", Password);
                    insertUserCommand.ExecuteNonQuery();

                    ModelState.AddModelError(string.Empty, "Đăng ký thành công");
                    ViewData["SuccessMessage"] = "Đăng ký thành công.";

                    return Page();
                }
            }

            return RedirectToPage("/Register/Register");
        }
    }
}