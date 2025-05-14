let employees = [
    { id: 1, name: "Diana Arreola", position: "Desarrollador", department: "IT", email: "diana@empresa.com" },
    { id: 2, name: "Argenis Esquivel", position: "Diseñador", department: "IT", email: "argenis@empresa.com" },
    { id: 3, name: "Alan Garcia", position: "Gerente", department: "IT", email: "alan@empresa.com" }
];

let currentId = 4;
let editingId = null;

// Función para mostrar los empleados en la tabla
function displayEmployees() {
    const tbody = document.querySelector('#employeesTable tbody');
    tbody.innerHTML = '';

    employees.forEach(employee => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${employee.id}</td>
            <td>${employee.name}</td>
            <td>${employee.position}</td>
            <td>${employee.department}</td>
            <td>${employee.email}</td>
            <td class="action-buttons">
                <button class="btn btn-edit" onclick="editEmployee(${employee.id})">Editar</button>
                <button class="btn btn-delete" onclick="deleteEmployee(${employee.id})">Eliminar</button>
            </td>
        `;
        tbody.appendChild(row);
    });
}

// Función para mostrar el modal de agregar empleado
function showAddModal() {
    document.getElementById('modalTitle').textContent = 'Agregar Empleado';
    document.getElementById('employeeForm').reset();
    document.getElementById('employeeModal').style.display = 'block';
    editingId = null;
}

// Función para cerrar el modal
function closeModal() {
    document.getElementById('employeeModal').style.display = 'none';
}

// Función para editar empleado
function editEmployee(id) {
    const employee = employees.find(emp => emp.id === id);
    if (employee) {
        document.getElementById('modalTitle').textContent = 'Editar Empleado';
        document.getElementById('name').value = employee.name;
        document.getElementById('position').value = employee.position;
        document.getElementById('department').value = employee.department;
        document.getElementById('email').value = employee.email;
        document.getElementById('employeeModal').style.display = 'block';
        editingId = id;
    }
}

// Función para eliminar empleado
function deleteEmployee(id) {
    if (confirm('¿Está seguro de que desea eliminar este empleado?')) {
        employees = employees.filter(emp => emp.id !== id);
        displayEmployees();
    }
}

// Event listener para el formulario
document.getElementById('employeeForm').addEventListener('submit', function (e) {
    e.preventDefault();

    const employeeData = {
        name: document.getElementById('name').value,
        position: document.getElementById('position').value,
        department: document.getElementById('department').value,
        email: document.getElementById('email').value
    };

    if (editingId === null) {
        // Agregar nuevo empleado
        employeeData.id = currentId++;
        employees.push(employeeData);
    } else {
        // Actualizar empleado existente
        const index = employees.findIndex(emp => emp.id === editingId);
        if (index !== -1) {
            employees[index] = { ...employees[index], ...employeeData };
        }
    }

    displayEmployees();
    closeModal();
});

// Event listener para la barra de búsqueda
document.querySelector('.search-bar').addEventListener('input', function (e) {
    const searchTerm = e.target.value.toLowerCase();
    const filteredEmployees = employees.filter(emp =>
        emp.name.toLowerCase().includes(searchTerm) ||
        emp.position.toLowerCase().includes(searchTerm) ||
        emp.department.toLowerCase().includes(searchTerm) ||
        emp.email.toLowerCase().includes(searchTerm)
    );

    const tbody = document.querySelector('#employeesTable tbody');
    tbody.innerHTML = '';

    filteredEmployees.forEach(employee => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${employee.id}</td>
            <td>${employee.name}</td>
            <td>${employee.position}</td>
            <td>${employee.department}</td>
            <td>${employee.email}</td>
            <td class="action-buttons">
                <button class="btn btn-edit" onclick="editEmployee(${employee.id})">Editar</button>
                <button class="btn btn-delete" onclick="deleteEmployee(${employee.id})">Eliminar</button>
            </td>
        `;
        tbody.appendChild(row);
    });
});

// Cargar los empleados iniciales
displayEmployees();