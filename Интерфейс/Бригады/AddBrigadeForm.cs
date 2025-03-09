using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace КурсоваяПрог.Интерфейс.Бригады
{
    public partial class AddBrigadeForm : Form
    {
        public Brigade NewBrigade { get; private set; } // Свойство для хранения новой бригады, доступное только для чтения извне
        private List<Brigade> existingBrigades; // Поле для хранения списка существующих бригад

        public AddBrigadeForm(List<Brigade> brigades) // Конструктор класса, принимающий список бригад
        {
            InitializeComponent(); // Инициализация компонентов формы
            existingBrigades = brigades; // Сохранение переданного списка бригад в поле
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(txtBrigadeName.Text) || string.IsNullOrWhiteSpace(txtForemanName.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения об ошибке
                return; // Выход из метода
            }

            // Проверка на наличие пробелов
            if (txtBrigadeName.Text.Trim().Length == 0 || txtForemanName.Text.Trim().Length == 0)
            {
                MessageBox.Show("Имя бригады и имя руководителя не могут состоять только из пробелов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения об ошибке
                return; // Выход из метода
            }

            // Проверка на наличие только цифр
            if (txtBrigadeName.Text.All(char.IsDigit) || txtForemanName.Text.All(char.IsDigit))
            {
                MessageBox.Show("Имя бригады и имя руководителя не могут состоять только из цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения об ошибке
                return; // Выход из метода
            }

            // Проверка на длину имени бригады
            if (txtBrigadeName.Text.Length < 1 || txtBrigadeName.Text.Length > 50)
            {
                MessageBox.Show("Имя бригады должно содержать от 1 до 50 символов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения об ошибке
                return; // Выход из метода
            }

            // Проверка на длину имени руководителя
            if (txtForemanName.Text.Length < 3 || txtForemanName.Text.Length > 50)
            {
                MessageBox.Show("Имя руководителя должно содержать от 3 до 50 символов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения об ошибке
                return; // Выход из метода
            }

            // Проверка на существование бригады с таким именем
            if (existingBrigades.Any(b => b.Name.Equals(txtBrigadeName.Text, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Бригада с таким именем уже существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Вывод сообщения об ошибке
                return; // Выход из метода
            }

            // Создание новой бригады
            NewBrigade = new Brigade // Инициализация нового объекта Brigade
            {
                Name = txtBrigadeName.Text, // Установка имени бригады из текстового поля
                Foreman = new Builder // Инициализация нового объекта Builder для руководителя
                {
                    FullName = txtForemanName.Text, // Установка полного имени руководителя из текстового поля
                    Gender = "Не указано", // Установка пола руководителя (можно добавить выбор пола)
                    BirthDate = DateTime.Now, // Установка даты рождения (можно добавить выбор даты)
                    Address = "Не указан", // Установка адреса (можно добавить поле для ввода адреса)
                    Experience = 0, // Установка опыта (можно добавить поле для ввода опыта)
                    Specialties = new List<string>() // Инициализация списка специальностей (можно добавить поле для ввода специальностей)
                }
            };

            DialogResult = DialogResult.OK; // Устанавливаем результат диалога как "ОК"
            Close(); // Закрываем форму
        }
    }
}
