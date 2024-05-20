<template>
  <div class="container">
    <div class="user-manager">
      <header>
        <img src="/to.png" alt="User Avatar" class="avatar">
        <span>Admin</span>
        <div class="search-container">
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" class="search-icon">
            <path
              d="M416 208c0 45.9-14.9 88.3-40 122.7L502.6 457.4c12.5 12.5 12.5 32.8 0 45.3s-32.8 12.5-45.3 0L330.7 376c-34.4 25.2-76.8 40-122.7 40C93.1 416 0 322.9 0 208S93.1 0 208 0S416 93.1 416 208zM208 352a144 144 0 1 0 0-288 144 144 0 1 0 0 288z" />
          </svg>
          <input type="text" placeholder="Search" v-model="searchQuery" class="search-input">
        </div>
        <button @click="login" class="login-button">Login</button>
      </header>
      <h2>User Management</h2>
      <div class="actions">
        <button @click="exportToExcel" class="export-add-button">
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 384 512" class="excel-icon">
            <path
              d="M64 0C28.7 0 0 28.7 0 64V448c0 35.3 28.7 64 64 64H320c35.3 0 64-28.7 64-64V160H256c-17.7 0-32-14.3-32-32V0H64zM256 0V128H384L256 0zM155.7 250.2L192 302.1l36.3-51.9c7.6-10.9 22.6-13.5 33.4-5.9s13.5 22.6 5.9 33.4L221.3 344l46.4 66.2c7.6 10.9 5 25.8-5.9 33.4s-25.8 5-33.4-5.9L192 385.8l-36.3 51.9c-7.6 10.9-22.6 13.5-33.4 5.9s-13.5-22.6-5.9-33.4L162.7 344l-46.4-66.2c-7.6-10.9-5-25.8 5.9-33.4s25.8-5 33.4 5.9z" />
          </svg>
          Export to Excel
        </button>
        <button @click="addNewUser" class="export-add-button">
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" class="add-icon">
            <path
              d="M256 512A256 256 0 1 0 256 0a256 256 0 1 0 0 512zM232 344V280H168c-13.3 0-24-10.7-24-24s10.7-24 24-24h64V168c0-13.3 10.7-24 24-24s24 10.7 24 24v64h64c13.3 0 24 10.7 24 24s-10.7 24-24 24H280v64c0 13.3-10.7 24-24 24s-24-10.7-24-24z" />
          </svg>
          Add New User
        </button>
      </div>
      <table>
        <thead>
          <tr>
            <th>#</th>
            <th>Name</th>
            <th>Email</th>
            <th>Password</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(user, index) in filteredUsers" :key="user.id"
            :class="{ 'odd-row': index % 2 === 0, 'even-row': index % 2 !== 0 }">
            <td>{{ index + 1 }}</td>
            <td class="user-name">
              <div class="user-info">
                <img :src="user.avatar" alt="Avatar" class="avatar-small">
                {{ user.name }}
              </div>
            </td>
            <td>{{ user.email }}</td>
            <td>{{ user.password }}</td>
            <td>
              <button @click="viewUser(user.id)" class="action-button view-icon">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 576 512">
                  <path
                    d="M288 32c-80.8 0-145.5 36.8-192.6 80.6C48.6 156 17.3 208 2.5 243.7c-3.3 7.9-3.3 16.7 0 24.6C17.3 304 48.6 356 95.4 399.4C142.5 443.2 207.2 480 288 480s145.5-36.8 192.6-80.6c46.8-43.5 78.1-95.4 93-131.1c3.3-7.9 3.3-16.7 0-24.6c-14.9-35.7-46.2-87.7-93-131.1C433.5 68.8 368.8 32 288 32zM144 256a144 144 0 1 1 288 0 144 144 0 1 1 -288 0zm144-64c0 35.3-28.7 64-64 64c-7.1 0-13.9-1.2-20.3-3.3c-5.5-1.8-11.9 1.6-11.7 7.4c.3 6.9 1.3 13.8 3.2 20.7c13.7 51.2 66.4 81.6 117.6 67.9s81.6-66.4 67.9-117.6c-11.1-41.5-47.8-69.4-88.6-71.1c-5.8-.2-9.2 6.1-7.4 11.7c2.1 6.4 3.3 13.2 3.3 20.3z" />
                </svg>
              </button>
              <button @click="editUser(user.id)" class="action-button edit-icon">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512">
                  <path
                    d="M471.6 21.7c-21.9-21.9-57.3-21.9-79.2 0L362.3 51.7l97.9 97.9 30.1-30.1c21.9-21.9 21.9-57.3 0-79.2L471.6 21.7zm-299.2 220c-6.1 6.1-10.8 13.6-13.5 21.9l-29.6 88.8c-2.9 8.6-.6 18.1 5.8 24.6s15.9 8.7 24.6 5.8l88.8-29.6c8.2-2.7 15.7-7.4 21.9-13.5L437.7 172.3 339.7 74.3 172.4 241.7zM96 64C43 64 0 107 0 160V416c0 53 43 96 96 96H352c53 0 96-43 96-96V320c0-17.7-14.3-32-32-32s-32 14.3-32 32v96c0 17.7-14.3 32-32 32H96c-17.7 0-32-14.3-32-32V160c0-17.7 14.3-32 32-32h96c17.7 0 32-14.3 32-32s-14.3-32-32-32H96z" />
                </svg>
              </button>
              <button @click="deleteUser(user.id)" class="action-button delete-icon">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512">
                  <path
                    d="M256 512A256 256 0 1 0 256 0a256 256 0 1 0 0 512zM175 175c9.4-9.4 24.6-9.4 33.9 0l47 47 47-47c9.4-9.4 24.6-9.4 33.9 0s9.4 24.6 0 33.9l-47 47 47 47c9.4 9.4 9.4 24.6 0 33.9s-24.6 9.4-33.9 0l-47-47-47 47c-9.4 9.4-24.6 9.4-33.9 0s-9.4-24.6 0-33.9l47-47-47-47c-9.4-9.4-9.4-24.6 0-33.9z" />
                </svg>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
      <div class="pagination">
        <button @click="prevPage" :disabled="currentPage === 5" class="pagination-info">
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512" class="icon-left">
            <path d="M9.4 233.4c-12.5 12.5-12.5 32.8 0 45.3l160 160c12.5 12.5 32.8 12.5 45.3 0s12.5-32.8 0-45.3L109.2 288 416 288c17.7 0 32-14.3 32-32s-14.3-32-32-32l-306.7 0L214.6 118.6c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0l-160 160z"/>
          </svg>
          Prev
        </button>
        <span v-for="page in totalPages" :key="page" @click="goToPage(page)"
          :class="{ active: currentPage === page }">{{ page }}</span>
        <button @click="nextPage" :disabled="currentPage === totalPages" class="pagination-info">
          Next
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512" class="icon-right">
            <path d="M438.6 278.6c12.5-12.5 12.5-32.8 0-45.3l-160-160c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L338.8 224 32 224c-17.7 0-32 14.3-32 32s14.3 32 32 32l306.7 0L233.4 393.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0l160-160z"/>
          </svg>
        </button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      searchQuery: '',
      currentPage: 1,
      usersPerPage: 5,
      users: [
        { id: 1, name: 'An Hoang', email: 'anhoang@gmail.com', password: 'hashedpassword1', avatar: '@/assets/avatar1.png' },
        { id: 2, name: 'Kha Nguyen', email: 'khanguyen@gmail.com', password: 'hashedpassword2', avatar: '@/assets/avatar2.png' },
        { id: 3, name: 'Tin Huynh', email: 'tinhuyhn@gmail.com', password: 'hashedpassword3', avatar: '@/assets/avatar3.png' },
        { id: 4, name: 'La Thanh', email: 'lathanh@gmail.com', password: 'hashedpassword4', avatar: '@/assets/avatar4.png' },
        { id: 5, name: 'Bang Pham', email: 'bangpham@gmail.com', password: 'hashedpassword5', avatar: '@/assets/avatar5.png' },
        // Add more users as needed
      ]
    };
  },
  computed: {
    filteredUsers() {
      const start = (this.currentPage - 1) * this.usersPerPage;
      const end = start + this.usersPerPage;
      return this.users.filter(user =>
        user.name.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        user.email.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        user.role.toLowerCase().includes(this.searchQuery.toLowerCase())
      ).slice(start, end);
    },
    totalPages() {
      return Math.ceil(this.users.length / this.usersPerPage);
    }
  },
  methods: {
    login() {

    },
    exportToExcel() {
      // Handle export to Excel functionality here
    },
    addNewUser() {
      // Handle adding a new user here
    },
    viewUser(userId) {
      // Handle viewing user details here
    },
    editUser(userId) {
      // Handle editing user details here
    },
    deleteUser(userId) {
      // Handle deleting a user here
    },
    prevPage() {
      if (this.currentPage > 1) this.currentPage--;
    },
    nextPage() {
      if (this.currentPage < this.totalPages) this.currentPage++;
    },
    goToPage(page) {
      this.currentPage = page;
    }
  }
};
</script>

<style scoped>
.container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background: linear-gradient(to bottom, #0081A7, #00AFB9);
}

.user-manager {
  width: 815px;
  border: 1px solid #ccc;
  padding: 40px;
  background-color: #fff;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  border-radius: 8px;
}

header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 15px;
  position: relative;
}

header span {
  font-size: 18px;
  font-weight: bold;
}

.search-container {
  position: absolute;
  left: 50%;
  transform: translateX(-50%);
  width: 300px;
  display: flex;
  align-items: center;
  border: 1px solid #ccc;
  border-radius: 30px;
  padding: 8px 12px;
}

.search-icon {
  width: 20px;
  height: 20px;
  margin-right: 10px;
  fill: #545454;
}

.search-input {
  width: 100%;
  border: none;
  outline: none;
  padding-left: 5px;
  border-radius: 20px;
}

.search-input::placeholder {
  color: #ccc;
}

.login-button {
  padding: 8px 16px;
  background-color: #299BE4;
  color: #fff;
  border: none;
  border-radius: 2px;
  cursor: pointer;
  font-size: 14px;
  font-weight: 600;
}

.login-button:hover {
  background-color: #268fd1;
}

.excel-icon {
  width: 15px;
  height: 15px;
  margin-right: 5px;
  fill: #566787;
}

.add-icon {
  width: 15px;
  height: 15px;
  margin-right: 5px;
  fill: #566787;
}

.export-add-button {
  display: flex;
  align-items: center;
  background-color: #fff;
  color: #566787;
  border: none;
  padding: 4px 10px;
  margin: -48px 0 17px 20px;
  cursor: pointer;
  border-radius: 2px;
  font-weight: 600;
  font-size: 12px;
}

header img {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  margin-right: 10px;
}

header span {
  font-size: 18px;
  font-weight: bold;
  margin-right: auto;
}

header input {
  padding: 5px;
  border: 1px solid #ccc;
  border-radius: 4px;
}

h2 {
  margin: 6px -41px 6px -41px;
  padding: 12px 0 12px 40px;
  font-size: 20px;
  font-weight: bold;
  color: #fff;
  background-color: #299BE4;
}

.actions {
  display: flex;
  justify-content: flex-end;
  margin-bottom: 10px;
}

.actions button {
  background-color: #fff;
  color: #566787;
  border: none;
  padding: 4px 10px;
  margin: -48px 0 17px 20px;
  cursor: pointer;
  border-radius: 2px;
  font-weight: 600;
  font-size: 12px;
}

.actions button:hover {
  background-color: #f4f4f4;
  transition: background 0.2s ease-in-out;
}

table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 10px;
}

th,
td {
  padding: 8px;
  text-align: left;
}

th {
  background-color: #fff;
  border-top: 1px solid #ddd;
  border-bottom: 2px solid #ddd;
}

td {
  border-bottom: 1px solid #ddd;
}

.user-name .user-info {
  display: flex;
  align-items: center;
}

.user-name .avatar-small {
  margin-right: 10px;
}

td button.action-button {
  background: none;
  border: none;
  cursor: pointer;
  padding: 5px;
  display: inline-flex;
  align-items: center;
  justify-content: center;
}

td button.action-button:hover {
  opacity: 0.7;
}

.action-button svg {
  width: 20px;
  height: 20px;
  fill: #299BE4;
  /* Màu xanh dương */
}

.action-button.view-icon svg {
  fill: #17a2b8;
  /* Màu xanh lơ */
}

.action-button.edit-icon svg {
  fill: #ffc107;
  /* Màu vàng */
}

.action-button.delete-icon svg {
  fill: #dc3545;
  /* Màu đỏ */
}

.odd-row td {
  background-color: #fcfcfc;
}

.even-row td {
  background-color: #fff;
}

.user-name {
  color: #566787;
}

td img {
  width: 30px;
  height: 30px;
  border-radius: 50%;
}

td button {
  background: none;
  border: none;
  cursor: pointer;
  padding: 5px;
}

td button:hover {
  opacity: 0.7;
}

.pagination {
  display: flex;
  justify-content: center;
  margin-top: 20px;
}

.pagination-info {
  display: flex;
  align-items: center;
  font-size: 16px;
}

.pagination-info svg {
  width: 15px;
  height: 15px; 
  fill: white; 
}

.icon-left {
  margin-right: 15px;
}

.icon-right {
  margin-left: 15px;
}

.pagination button {
  margin: 0 15px;
  background-color: #007bff;
  color: white;
  border: none;
  padding: 8px 12px;
  cursor: pointer;
  border-radius: 4px;
}

.pagination button:disabled {
  background-color: #ccc;
}

.pagination .active {
  font-weight: bold;
  padding: 5px 5px;
  /* font-size: 20px; */
}
</style>
