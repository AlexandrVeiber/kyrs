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
    public partial class EditWorkerForm : Form
    {
        public Builder UpdatedWorker { get; private set; } // Добавляем свойство для обновленного работника

        public EditWorkerForm(Builder worker)
        {
            InitializeComponent(); // Инициализация компонентов формы
            UpdatedWorker = worker; // Сохраняем ссылку на редактируемого работника

            // Заполнение полей данными работника
            txtFullName.Text = worker.FullName; // Устанавливаем полное имя работника
            comboBoxGender.SelectedItem = worker.Gender; // Устанавливаем выбранный пол
            dtpBirthDate.Value = worker.BirthDate; // Устанавливаем дату рождения
            txtAddress.Text = worker.Address; // Устанавливаем адрес
            numExperience.Value = worker.Experience; // Устанавливаем опыт
            txtSpecialties.Text = string.Join(", ", worker.Specialties); // Устанавливаем специальности

            // Инициализация ComboBox для выбора пола
            InitializeGenderComboBox(); // Вызываем метод для инициализации ComboBox
        }

        private void InitializeGenderComboBox()
        {
            // Добавляем значения в ComboBox
            comboBoxGender.Items.Add("Мужской");
            comboBoxGender.Items.Add("Женский");
            comboBoxGender.Items.Add("Не указано");
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            // Проверка на заполненность обязательных полей
            if (string.IsNullOrWhiteSpace(txtFullName.Text) || // Проверяем, заполнено ли полное имя
                comboBoxGender.SelectedItem == null || // Проверка на выбор пола
                string.IsNullOrWhiteSpace(txtAddress.Text)) // Проверяем, заполнен ли адрес
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // Сообщение об ошибке
                return; // Выход из метода
            }

            // Проверка на наличие пробелов
            if (txtFullName.Text.Trim().Length == 0 || txtAddress.Text.Trim().Length == 0) // Если имя или адрес состоят только из пробелов
            {
                MessageBox.Show("Имя работника и адрес не могут состоять только из пробелов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Сообщение об ошибке
                return; // Выход из метода
            }

            // Проверка на наличие только цифр в имени работника
            if (txtFullName.Text.All(char.IsDigit)) // Если полное имя состоит только из цифр
            {
                MessageBox.Show("Имя работника не может состоять только из цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Сообщение об ошибке
                return; // Выход из метода
            }

            // Проверка на длину имени работника
            if (txtFullName.Text.Length < 1 || txtFullName.Text.Length > 50) // Если длина имени вне допустимого диапазона
            {
                MessageBox.Show("Имя работника должно содержать от 1 до 50 символов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Сообщение об ошибке
                return; // Выход из метода
            }

            // Проверка на специальности
            var specialties = txtSpecialties.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries) // Разделяем специальности по запятой
                                                  .Select(s => s.Trim()) // Убираем пробелы
                                                  .ToList(); // Преобразуем в список
            if (specialties.Any(s => string.IsNullOrWhiteSpace(s))) // Проверяем, есть ли пустые специальности
            {
                MessageBox.Show("Специальности не могут состоять только из пробелов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Сообщение об ошибке
                return; // Выход из метода
            }

            // Обновление данных работника
            UpdatedWorker.FullName = txtFullName.Text.Trim(); // Устанавливаем обновленное полное имя
            UpdatedWorker.Gender = comboBoxGender.SelectedItem.ToString(); // Получаем выбранный пол
            UpdatedWorker.BirthDate = dtpBirthDate.Value; // Устанавливаем дату рождения
            UpdatedWorker.Address = txtAddress.Text.Trim(); // Устанавливаем обновленный адрес
            UpdatedWorker.Experience = (int)numExperience.Value; // Устанавливаем обновленный опыт
            UpdatedWorker.Specialties = specialties; // Устанавливаем обновленные специальности

            DialogResult = DialogResult.OK; // Устанавливаем результат диалога как OK
            Close(); // Закрываем форму
        }
    }
}