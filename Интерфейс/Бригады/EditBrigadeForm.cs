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
    public partial class EditBrigadeForm : Form
    {
        public Brigade UpdatedBrigade { get; private set; } // Добавлено свойство для возвращения обновленной бригады
        private Brigade brigadeToEdit; // Хранит редактируемую бригаду
        public EditBrigadeForm(Brigade brigade)
        {
            InitializeComponent(); // Инициализация компонентов формы

            // Проверка на null
            if (brigade == null) // Если переданная бригада равна null
            {
                throw new ArgumentNullException(nameof(brigade), "Бригада не может быть null"); // Генерация исключения
            }

            brigadeToEdit = brigade; // Сохраняем ссылку на редактируемую бригаду

            // Заполнение полей данными бригады
            txtBrigadeName.Text = brigade.Name; // Устанавливаем имя бригады в текстовое поле

            // Проверка на null для Foreman
            if (brigade.Foreman != null) // Если у бригады есть руководитель
            {
                txtForemanName.Text = brigade.Foreman.FullName; // Устанавливаем имя руководителя в текстовое поле
            }
            else
            {
                txtForemanName.Text = string.Empty; // Оставляем поле пустым, если руководитель отсутствует
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(txtBrigadeName.Text))
            {
                MessageBox.Show("Пожалуйста, введите имя бригады.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка на наличие пробелов
            if (txtBrigadeName.Text.Trim().Length == 0)
            {
                MessageBox.Show("Имя бригады не может состоять только из пробелов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка на наличие только цифр в имени бригады
            if (txtBrigadeName.Text.All(char.IsDigit))
            {
                MessageBox.Show("Имя бригады не может состоять только из цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка на длину имени бригады
            if (txtBrigadeName.Text.Length < 3 || txtBrigadeName.Text.Length > 50)
            {
                MessageBox.Show("Имя бригады должно содержать от 3 до 50 символов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Обновление данных бригады
            brigadeToEdit.Name = txtBrigadeName.Text.Trim();

            // Проверка на null перед обновлением имени руководителя
            if (brigadeToEdit.Foreman != null)
            {
                if (string.IsNullOrWhiteSpace(txtForemanName.Text))
                {
                    MessageBox.Show("Пожалуйста, введите имя руководителя.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Выход из метода, если имя руководителя не указано
                }

                // Проверка на наличие пробелов
                if (txtForemanName.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Имя руководителя не может состоять только из пробелов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Проверка на наличие только цифр в имени руководителя
                if (txtForemanName.Text.All(char.IsDigit))
                {
                    MessageBox.Show("Имя руководителя не может состоять только из цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Проверка на длину имени руководителя
                if (txtForemanName.Text.Length < 3 || txtForemanName.Text.Length > 50)
                {
                    MessageBox.Show("Имя руководителя должно содержать от 3 до 50 символов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                brigadeToEdit.Foreman.FullName = txtForemanName.Text.Trim();
            }
            else
            {
                // Если Foreman равен null, создаем нового Foreman, если имя руководителя не пустое
                if (!string.IsNullOrWhiteSpace(txtForemanName.Text))
                {
                    brigadeToEdit.Foreman = new Builder { FullName = txtForemanName.Text.Trim() }; // Предполагается, что Builder имеет свойство FullName
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите имя руководителя.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Выход из метода, если имя руководителя не указано
                }
            }

            // Устанавливаем обновленную бригаду
            UpdatedBrigade = brigadeToEdit;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}