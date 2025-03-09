using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace КурсоваяПрог.Интерфейс.склад.поставщики
{
    public partial class AddSupplierForm : Form
    {
        public Supplier NewSupplier { get; private set; } // Свойство для возвращения нового поставщика

        public AddSupplierForm(Supplier supplier = null) // Конструктор класса, принимающий поставщика для редактирования
        {
            InitializeComponent(); // Инициализация компонентов формы

            if (supplier != null) // Если передан существующий поставщик
            {
                // Заполнение полей данными поставщика
                textBoxName.Text = supplier.Name; // Устанавливаем имя поставщика
                textBoxAddress.Text = supplier.Address; // Устанавливаем адрес поставщика
                textBoxDirectorName.Text = supplier.DirectorName; // Устанавливаем имя директора
                textBoxDirectorPhone.Text = supplier.DirectorPhone; // Устанавливаем номер телефона директора
                textBoxBank.Text = supplier.Bank; // Устанавливаем название банка
                textBoxAccount.Text = supplier.Account; // Устанавливаем номер счета
                textBoxINN.Text = supplier.INN; // Устанавливаем ИНН
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            // Проверка на обязательные поля
            if (string.IsNullOrWhiteSpace(textBoxName.Text) || string.IsNullOrWhiteSpace(textBoxAddress.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxDirectorName.Text))
            {
                MessageBox.Show("Пожалуйста, укажите имя директора.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка ИНН
            if (!IsValidINN(textBoxINN.Text))
            {
                MessageBox.Show("ИНН должен содержать 10 или 12 цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка номера телефона
            if (!IsValidPhoneNumber(textBoxDirectorPhone.Text))
            {
                MessageBox.Show("Номер телефона должен содержать 10 цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка на название банка
            if (string.IsNullOrWhiteSpace(textBoxBank.Text))
            {
                MessageBox.Show("Пожалуйста, укажите название банка.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка номера счета
            if (!IsValidAccount(textBoxAccount.Text))
            {
                MessageBox.Show("Номер счета должен содержать 20 цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка, что имя и адрес не состоят только из цифр
            if (textBoxName.Text.Trim().All(char.IsDigit) || textBoxAddress.Text.Trim().All(char.IsDigit))
            {
                MessageBox.Show("Имя и адрес не могут состоять только из цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Создание нового поставщика
            NewSupplier = new Supplier // Инициализация нового объекта Supplier
            {
                Name = textBoxName.Text.Trim(), // Убираем пробелы в начале и конце
                Address = textBoxAddress.Text.Trim(), // Убираем пробелы в начале и конце
                DirectorName = textBoxDirectorName.Text.Trim(), // Убираем пробелы в начале и конце
                DirectorPhone = textBoxDirectorPhone.Text.Trim(), // Убираем пробелы в начале и конце
                Bank = textBoxBank.Text.Trim(), // Убираем пробелы в начале и конце
                Account = textBoxAccount.Text.Trim(), // Убираем пробелы в начале и конце
                INN = textBoxINN.Text.Trim() // Убираем пробелы в начале и конце
            };

            DialogResult = DialogResult.OK;
            Close();
        }

        // Обработчик для предотвращения ввода недопустимых символов
        private void textBoxDirectorPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем ввод только цифр
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Отменяем ввод
            }
        }

        private void textBoxAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем ввод только цифр
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Отменяем ввод
            }
        }

        private void textBoxINN_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем ввод только цифр
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Отменяем ввод
            }
        }


        private bool IsValidINN(string inn)
        {
            // Проверка на 10 или 12 цифр
            return Regex.IsMatch(inn, @"^\d{10}$") || Regex.IsMatch(inn, @"^\d{12}$");
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Проверка на 10 цифр
            return Regex.IsMatch(phoneNumber, @"^\d{10}$");
        }

        private bool IsValidAccount(string account)
        {
            // Проверка на 20 цифр
            return Regex.IsMatch(account, @"^\d{20}$");
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void AddSupplierForm_Load(object sender, EventArgs e)
        {

        }
    }
}
