using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace КурсоваяПрог.Интерфейс.Бригады
{
    public partial class BrigadeForm : Form
    {
        private Project project; // Хранит проект, к которому относится бригада
        private Brigade selectedBrigade; // Хранит выбранную бригаду
        private List<Project> projects; // Хранит список проектов
        private DataManager dataManager = new DataManager(); // Создаем экземпляр DataManager для работы с данными
        private const string DataDirectoryPath = "Data"; // Путь к директории для сохранения данных

        public BrigadeForm(Project initialProject, List<Project> projects) // Конструктор класса, принимающий начальный проект и список проектов
        {
            InitializeComponent(); // Инициализация компонентов формы
            this.project = initialProject; // Сохраняем выбранный проект
            this.projects = projects; // Сохраняем список проектов

            InitializeComponents(); // Инициализация компонентов формы
            UpdateBrigadeDataGridView(); // Обновляем данные о бригадах
            UpdateBrigadeComboBox(); // Обновляем ComboBox для бригад
        }

        private void InitializeComponents()
        {
            InitializeProjectComboBox(); // Инициализация ComboBox для проектов
            InitializeBrigadeComboBox(); // Инициализация ComboBox для бригад
            InitializeDataGridViewBrigades(); // Инициализация DataGridView для бригад
            InitializeDataGridViewWorkers(); // Инициализация DataGridView для работников
        }


        private void SaveBrigadesToFile()
        {
            // Сохраняем все проекты, включая текущий
            dataManager.SaveAllData(DataDirectoryPath, projects, new List<Builder>(), new List<CompletedWork>(), new List<Delivery>(), new List<ConstructionMaterial>(), new List<Request>());
        }

        private void InitializeProjectComboBox()
        {
            comboBoxProject.DataSource = projects; // Устанавливаем источник данных для ComboBox
            comboBoxProject.DisplayMember = "Name"; // Указываем, что отображаем имя проекта
            comboBoxProject.SelectedIndexChanged += (s, e) =>
            {
                // Обновляем выбранный проект и данные о бригадах
                project = comboBoxProject.SelectedItem as Project;
                UpdateBrigadeDataGridView(); // Обновляем данные о бригадах для нового проекта
                UpdateBrigadeComboBox(); // Обновляем ComboBox для бригад
            };
        }

        private void InitializeBrigadeComboBox()
        {
            comboBoxBrigade.DisplayMember = "Name"; // Указываем, что отображаем имя бригады
            comboBoxBrigade.SelectedIndexChanged += (s, e) =>
            {
                // Обновляем выбранную бригаду
                selectedBrigade = comboBoxBrigade.SelectedItem as Brigade;
                UpdateWorkerListBox(); // Обновляем список работников
                UpdateWorkerDataGridView(); // Обновляем DataGridView для работников
            };
            UpdateBrigadeComboBox(); // Обновляем ComboBox для бригад
        }





        private void UpdateBrigadeComboBox()
        {
            if (project != null)
            {
                comboBoxBrigade.DataSource = null; // Сбрасываем источник данных
                comboBoxBrigade.DataSource = project.Brigades; // Устанавливаем источник данных для ComboBox
                comboBoxBrigade.DisplayMember = "Name"; // Указываем, что отображаем имя бригады
                comboBoxBrigade.SelectedIndex = -1; // Сбрасываем выбор
            }
        }




        private void InitializeDataGridViewBrigades()
        {
            dataGridViewBrigades.AutoGenerateColumns = false; // Отключаем автоматическую генерацию столбцов

            // Добавляем столбцы в DataGridView
            dataGridViewBrigades.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Имя бригады",
                DataPropertyName = "BrigadeName", // Имя бригады
                Width = 150 // Устанавливаем ширину столбца
            });
            dataGridViewBrigades.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Имя руководителя",
                DataPropertyName = "ForemanName", // Имя руководителя
                Width = 150 // Устанавливаем ширину столбца
            });
            dataGridViewBrigades.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Количество работников",
                DataPropertyName = "WorkerCount", // Количество работников
                Width = 150 // Устанавливаем ширину столбца
            });

            // Устанавливаем режим автоматической подгонки ширины столбцов
            dataGridViewBrigades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Заполняем доступное пространство
        }

        private List<Brigade> LoadBrigades() // Метод для загрузки бригад
        {
            return project.Brigades ?? new List<Brigade>(); // Возвращаем список бригад или пустой список, если бригад нет
        }






        private void UpdateBrigadeDataGridView() // Метод для обновления DataGridView бригад
        {
            if (project != null) // Проверяем, выбран ли проект
            {
                var brigadesToDisplay = project.Brigades.Select(b => new // Формируем список для отображения
                {
                    Brigade = b, // Ссылка на объект бригады
                    BrigadeName = b.Name, // Имя бригады
                    ForemanName = b.Foreman?.FullName ?? "Не указано", // Имя руководителя или "Не указано"
                    WorkerCount = b.Workers?.Count ?? 0 // Количество работников или 0
                }).ToList();

                dataGridViewBrigades.DataSource = brigadesToDisplay; // Устанавливаем источник данных для DataGridView
                dataGridViewBrigades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Устанавливаем режим автоматической подгонки ширины столбцов

                if (brigadesToDisplay.Count == 0) // Проверяем, есть ли бригады для отображения
                {
                    MessageBox.Show("Нет доступных бригад для отображения в выбранном проекте.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information); // Сообщение об отсутствии бригад
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите проект.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Сообщение об ошибке
                dataGridViewBrigades.DataSource = null; // Сбрасываем источник данных
            }
        }



        private void btnAddBrigade_Click(object sender, EventArgs e)
        {
            using (AddBrigadeForm addBrigadeForm = new AddBrigadeForm(project.Brigades))
            {
                if (addBrigadeForm.ShowDialog() == DialogResult.OK)
                {
                    Brigade newBrigade = addBrigadeForm.NewBrigade;
                    newBrigade.AssignedProject = project; // Привязываем бригаду к проекту
                    project.Brigades.Add(newBrigade); // Добавляем бригаду в проект
                    SaveBrigadesToFile(); // Сохраняем изменения в файл
                    UpdateBrigadeDataGridView(); // Обновляем отображение бригад
                    UpdateBrigadeComboBox(); // Обновляем ComboBox бригад
                    comboBoxBrigade.SelectedItem = newBrigade; // Устанавливаем только что добавленную бригаду как выбранную
                }
            }
        }
        private void btnEditBrigade_Click(object sender, EventArgs e)
        {
            if (comboBoxBrigade.SelectedItem is Brigade selectedBrigade) // Проверяем, выбрана ли бригада
            {
                using (EditBrigadeForm editBrigadeForm = new EditBrigadeForm(selectedBrigade)) // Создаем форму для редактирования бригады
                {
                    if (editBrigadeForm.ShowDialog() == DialogResult.OK) // Проверяем, был ли результат "ОК"
                    {
                        selectedBrigade.Name = editBrigadeForm.UpdatedBrigade.Name; // Обновляем имя бригады
                        selectedBrigade.Foreman = editBrigadeForm.UpdatedBrigade.Foreman; // Обновляем руководителя бригады
                        SaveBrigadesToFile(); // Сохраняем изменения в файл
                        UpdateBrigadeDataGridView(); // Обновляем отображение бригад
                        UpdateBrigadeComboBox(); // Обновляем ComboBox бригад
                    }
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите бригаду для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Сообщение об ошибке
            }
        }

        private void btnDeleteBrigade_Click(object sender, EventArgs e)
        {
            if (dataGridViewBrigades.SelectedRows.Count > 0) // Проверяем, выбрана ли бригада
            {
                var selectedBrigadeDisplay = (dynamic)dataGridViewBrigades.SelectedRows[0].DataBoundItem; // Получаем выбранную бригаду
                Brigade selectedBrigade = selectedBrigadeDisplay.Brigade; // Ссылка на объект бригады

                if (MessageBox.Show("Вы уверены, что хотите удалить эту бригаду?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes) // Подтверждение удаления
                {
                    if (selectedBrigade.Workers.Count > 0) // Проверяем, есть ли работники в бригаде
                    {
                        MessageBox.Show("Невозможно удалить бригаду, пока в ней есть работники.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Сообщение об ошибке
                        return; // Выход из метода
                    }

                    project.Brigades.Remove(selectedBrigade); // Удаляем бригаду из проекта
                    SaveBrigadesToFile(); // Сохраняем изменения в файл
                    UpdateBrigadeDataGridView(); // Обновляем отображение бригад
                    UpdateBrigadeComboBox(); // Обновляем ComboBox бригад
                    comboBoxBrigade.SelectedIndex = -1; // Сбрасываем выбор в ComboBox
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите бригаду для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Сообщение об ошибке
            }
        }






        ////////////////////////////////////////
        // РАБОТНИКИ

        private void btnAddWorker_Click(object sender, EventArgs e)
        {
            if (selectedBrigade != null) // Проверяем, выбрана ли бригада
            {
                using (AddWorkerForm addWorkerForm = new AddWorkerForm(selectedBrigade)) // Создаем форму для добавления работника
                {
                    if (addWorkerForm.ShowDialog() == DialogResult.OK) // Проверяем, был ли результат "ОК"
                    {
                        UpdateWorkerListBox(); // Обновляем список работников
                        UpdateWorkerDataGridView(); // Обновляем DataGridView для работников
                        SaveBrigadesToFile(); // Сохраняем изменения в файл
                    }
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите бригаду.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Сообщение об ошибке
            }
        }

        private void btnEditWorker_Click(object sender, EventArgs e)
        {
            if (listBoxWorkers.SelectedItem is Builder selectedWorker) // Проверяем, выбран ли работник
            {
                using (EditWorkerForm editWorkerForm = new EditWorkerForm(selectedWorker)) // Создаем форму для редактирования работника
                {
                    if (editWorkerForm.ShowDialog() == DialogResult.OK) // Проверяем, был ли результат "ОК"
                    {
                        // Обновляем свойства работника напрямую
                        selectedWorker.FullName = editWorkerForm.UpdatedWorker.FullName; // Обновляем полное имя работника
                        selectedWorker.Gender = editWorkerForm.UpdatedWorker.Gender; // Обновляем пол работника
                        selectedWorker.BirthDate = editWorkerForm.UpdatedWorker.BirthDate; // Обновляем дату рождения работника
                        selectedWorker.Address = editWorkerForm.UpdatedWorker.Address; // Обновляем адрес работника
                        selectedWorker.Experience = editWorkerForm.UpdatedWorker.Experience; // Обновляем опыт работника
                        selectedWorker.Specialties = editWorkerForm.UpdatedWorker.Specialties; // Обновляем специальности работника

                        UpdateWorkerListBox(); // Обновляем список работников
                        UpdateWorkerDataGridView(); // Обновляем DataGridView для работников
                        SaveBrigadesToFile(); // Сохраняем изменения в файл
                    }
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите работника для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Сообщение об ошибке
            }
        }

        private void btnDeleteWorker_Click(object sender, EventArgs e)
        {
            if (selectedBrigade != null && listBoxWorkers.SelectedItem is Builder selectedWorker) // Проверяем, выбраны ли бригада и работник
            {
                if (MessageBox.Show($"Вы уверены, что хотите удалить работника {selectedWorker.FullName}?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes) // Подтверждение удаления
                {
                    selectedBrigade.Workers.Remove(selectedWorker); // Удаляем работника из бригады
                    UpdateWorkerListBox(); // Обновляем список работников
                    UpdateWorkerDataGridView(); // Обновляем DataGridView для работников
                    SaveBrigadesToFile(); // Сохраняем изменения в файл
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите бригаду и работника для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Сообщение об ошибке
            }
        }

        private void InitializeDataGridViewWorkers()
        {
            dataGridViewWorkers.AutoGenerateColumns = false;
            dataGridViewWorkers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ФИО", DataPropertyName = "FullName" });
            dataGridViewWorkers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Пол", DataPropertyName = "Gender" });
            dataGridViewWorkers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Дата рождения", DataPropertyName = "BirthDate" });
            dataGridViewWorkers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Адрес", DataPropertyName = "Address" });
            dataGridViewWorkers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Опыт", DataPropertyName = "Experience" });
            dataGridViewWorkers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Специальности", DataPropertyName = "Specialties" });
        }


        private void UpdateWorkerListBox()
        {
            if (selectedBrigade == null)
            {
                listBoxWorkers.DataSource = null; // Сбросить источник данных
                return; // Выход из метода, если бригада не выбрана
            }

            listBoxWorkers.DataSource = selectedBrigade.Workers; // Обновить источник данных
            listBoxWorkers.DisplayMember = "FullName"; // Отображаем ФИО в ListBox
        }

        private void UpdateWorkerDataGridView() // Метод для обновления DataGridView работников
        {
            if (selectedBrigade != null) // Проверяем, выбрана ли бригада
            {
                var workersToDisplay = selectedBrigade.Workers.Select(w => new // Формируем список для отображения
                {
                    w.Id, // Идентификатор работника
                    w.FullName, // Полное имя работника
                    w.Gender, // Пол работника
                    w.BirthDate, // Дата рождения работника
                    w.Address, // Адрес работника
                    w.Experience, // Опыт работника
                    Specialties = string.Join(", ", w.Specialties) // Специальности работника, объединенные в строку
                }).ToList();

                dataGridViewWorkers.DataSource = workersToDisplay; // Устанавливаем источник данных для DataGridView

                if (workersToDisplay.Count == 0) // Проверяем, есть ли работники для отображения
                {
                    // Здесь можно добавить сообщение об отсутствии работников
                }
            }
            else
            {
                dataGridViewWorkers.DataSource = null; // Сбрасываем источник данных, если бригада не выбрана
            }
        }
    }
}