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
    public partial class AddWorkerForm : Form
    {
        public Builder NewWorker { get; private set; }
        private Brigade selectedBrigade; // Поле для хранения выбранной бригады



        public AddWorkerForm(Brigade selectedBrigade)
        {
            InitializeComponent();
            this.selectedBrigade = selectedBrigade; // Сохраняем ссылку на выбранную бригаду

            // Инициализация ComboBox для выбора пола
            InitializeGenderComboBox();
        }

        private void InitializeGenderComboBox()
        {
            // Добавляем значения в ComboBox
            comboBoxGender.Items.Add("Мужской");
            comboBoxGender.Items.Add("Женский");
            comboBoxGender.Items.Add("Не указано");
            comboBoxGender.SelectedIndex = 0; // Устанавливаем значение по умолчанию
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            // Проверка на заполненность обязательных полей
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                comboBoxGender.SelectedItem == null || // Проверка на выбор пола
                string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Проверка на наличие пробелов
            if (txtFullName.Text.Trim().Length == 0 || txtAddress.Text.Trim().Length == 0)
            {
                MessageBox.Show("Имя работника и адрес не могут состоять только из пробелов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка на наличие только цифр в имени работника
            if (txtFullName.Text.All(char.IsDigit))
            {
                MessageBox.Show("Имя работника не может состоять только из цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка на длину имени работника
            if (txtFullName.Text.Length < 1 || txtFullName.Text.Length > 50)
            {
                MessageBox.Show("Имя работника должно содержать от 1 до 50 символов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка на специальности
            var specialties = txtSpecialties.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(s => s.Trim())
                                                  .ToList();
            if (specialties.Any(s => string.IsNullOrWhiteSpace(s)))
            {
                MessageBox.Show("Специальности не могут состоять только из пробелов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Создание нового работника
            var newWorker = new Builder
            {
                FullName = txtFullName.Text.Trim(),
                Gender = comboBoxGender.SelectedItem.ToString(), // Получаем выбранный пол
                BirthDate = dtpBirthDate.Value,
                Address = txtAddress.Text.Trim(),
                Experience = (int)numExperience.Value,
                Specialties = specialties,
                Brigade = selectedBrigade // Устанавливаем бригаду для работника
            };

            // Добавление работника в выбранную бригаду
            selectedBrigade.Workers.Add(newWorker);

            // Закрытие формы с результатом OK
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
