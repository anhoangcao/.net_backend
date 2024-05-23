<template>
  <div class="container">
    <div class="user-manager">
      <header>
        <img src="/ava.jpg" alt="User Avatar" class="avatar">
        <span>{{ userName }}</span>
        <div class="search-container">
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" class="search-icon">
            <path
              d="M416 208c0 45.9-14.9 88.3-40 122.7L502.6 457.4c12.5 12.5 12.5 32.8 0 45.3s-32.8 12.5-45.3 0L330.7 376c-34.4 25.2-76.8 40-122.7 40C93.1 416 0 322.9 0 208S93.1 0 208 0S416 93.1 416 208zM208 352a144 144 0 1 0 0-288 144 144 0 1 0 0 288z" />
          </svg>
          <input type="text" placeholder="Search" v-model="searchQuery" class="search-input">
        </div>
        <button @click="logout" class="logout-button">Logout</button>
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
            <th>Update Date</th>
            <th>Create Date</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="filteredUsers.length === 0">
            <td colspan="7" class="no-data">Không có dữ liệu</td>
          </tr>
          <tr v-else v-for="(user, index) in filteredUsers" :key="user.userID"
            :class="{ 'odd-row': index % 2 === 0, 'even-row': index % 2 !== 0 }">
            <td>{{ index + 1 + (currentPage - 1) * usersPerPage }}</td>
            <td class="user-name">
              <div class="user-info">
                {{ user.name }}
              </div>
            </td>
            <td>{{ user.email }}</td>
            <td>{{ truncatedPassword(user.passwordHash) }}</td>
            <td>{{ new Date(user.updatedAt).toLocaleString() }}</td>
            <td>{{ new Date(user.createdAt).toLocaleString() }}</td>
            <td>
              <button @click="viewUser(user.userID)" class="action-button view-icon">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 576 512">
                  <path
                    d="M288 32c-80.8 0-145.5 36.8-192.6 80.6C48.6 156 17.3 208 2.5 243.7c-3.3 7.9-3.3 16.7 0 24.6C17.3 304 48.6 356 95.4 399.4C142.5 443.2 207.2 480 288 480s145.5-36.8 192.6-80.6c46.8-43.5 78.1-95.4 93-131.1c3.3-7.9 3.3-16.7 0-24.6c-14.9-35.7-46.2-87.7-93-131.1C433.5 68.8 368.8 32 288 32zM144 256a144 144 0 1 1 288 0 144 144 0 1 1 -288 0zm144-64c0 35.3-28.7 64-64 64c-7.1 0-13.9-1.2-20.3-3.3c-5.5-1.8-11.9 1.6-11.7 7.4c.3 6.9 1.3 13.8 3.2 20.7c13.7 51.2 66.4 81.6 117.6 67.9s81.6-66.4 67.9-117.6c-11.1-41.5-47.8-69.4-88.6-71.1c-5.8-.2-9.2 6.1-7.4 11.7c2.1 6.4 3.3 13.2 3.3 20.3z" />
                </svg>
              </button>
              <button @click="editUser(user.userID)" class="action-button edit-icon">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512">
                  <path
                    d="M471.6 21.7c-21.9-21.9-57.3-21.9-79.2 0L362.3 51.7l97.9 97.9 30.1-30.1c21.9-21.9 21.9-57.3 0-79.2L471.6 21.7zm-299.2 220c-6.1 6.1-10.8 13.6-13.5 21.9l-29.6 88.8c-2.9 8.6-.6 18.1 5.8 24.6s15.9 8.7 24.6 5.8l88.8-29.6c8.2-2.7 15.7-7.4 21.9-13.5L437.7 172.3 339.7 74.3 172.4 241.7zM96 64C43 64 0 107 0 160V416c0 53 43 96 96 96H352c53 0 96-43 96-96V320c0-17.7-14.3-32-32-32s-32 14.3-32 32v96c0 17.7-14.3 32-32 32H96c-17.7 0-32-14.3-32-32V160c0-17.7 14.3-32 32-32h96c17.7 0 32-14.3 32-32s-14.3-32-32-32H96z" />
                </svg>
              </button>
              <button @click="deleteUser(user.userID)" class="action-button delete-icon">
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
        <button @click="prevPage" :disabled="currentPage === 1" class="pagination-info">
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 320 512" class="icon-left">
            <path
              d="M9.4 233.4c-12.5 12.5-12.5 32.8 0 45.3l192 192c12.5 12.5 32.8 12.5 45.3 0s12.5-32.8 0-45.3L77.3 256 246.6 86.6c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0l-192 192z" />
          </svg>
          Prev
        </button>
        <span v-for="page in visiblePages" :key="page" @click="goToPage(page)"
          :class="{ active: currentPage === page }">{{ page }}</span>
        <button @click="nextPage" :disabled="currentPage >= totalPages" class="pagination-info">
          Next
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 320 512" class="icon-right">
            <path
              d="M310.6 233.4c12.5 12.5 12.5 32.8 0 45.3l-192 192c-12.5 12.5-32.8 12.5-45.3 0s-12.5-32.8 0-45.3L242.7 256 73.4 86.6c-12.5-12.5-12.5-32.8 0-45.3s32.8-12.5 45.3 0l192 192z" />
          </svg>
        </button>
      </div>
    </div>
    <!-- Modal Background -->
    <div v-if="showUserDetail" class="modal-background"></div>
      
    <!-- User Detail Modal -->
    <div v-if="showUserDetail" class="user-detail-modal">
      <h3>User Details</h3>
      <p><strong>Name:</strong> {{ selectedUser.name }}</p>
      <p><strong>Email:</strong> {{ selectedUser.email }}</p>
      <p><strong>Update Date:</strong> {{ new Date(selectedUser.updatedAt).toLocaleString() }}</p>
      <p><strong>Create Date:</strong> {{ new Date(selectedUser.createdAt).toLocaleString() }}</p>
      <button @click="closeUserDetail" class="close-button">x</button>
    </div>
  </div>
</template>

<script>
import * as XLSX from 'xlsx';
import axios from 'axios';

export default {
  data() {
    return {
      searchQuery: '',
      currentPage: 1,
      usersPerPage: 5,
      users: [],
      totalPages: 1,
      userName: localStorage.getItem('userName') || 'Admin',
      showUserDetail: false,
      selectedUser: {}
    };
  },
  computed: {
    filteredUsers() {
      const filtered = this.users.filter(user =>
        user.name.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        user.email.toLowerCase().includes(this.searchQuery.toLowerCase())
      );
      return filtered;
    },
    visiblePages() {
      const pages = [];
      if (this.totalPages <= 4) {
        for (let i = 1; i <= this.totalPages; i++) {
          pages.push(i);
        }
      } else {
        if (this.currentPage <= 2) {
          pages.push(1, 2, '...', this.totalPages);
        } else if (this.currentPage >= this.totalPages - 1) {
          pages.push(1, '...', this.totalPages - 1, this.totalPages);
        } else {
          pages.push(1, '...', this.currentPage, '...', this.totalPages);
        }
      }
      return pages;
    }
  },
  methods: {
    logout() {
      localStorage.removeItem('userToken');
      localStorage.removeItem('userName');
      this.userName = 'Admin';
      this.$router.push('/login');
    },
    async fetchUsers(pageNumber = 1, pageSize = 5) {
      try {
        const response = await axios.get(`https://localhost:44345/api/users?pageNumber=${pageNumber}&pageSize=${pageSize}`);
        if (response.data) {
          this.users = response.data.users;
          this.totalPages = response.data.totalPages;
          this.currentPage = response.data.currentPage;
        } else {
          this.users = [];
          this.totalPages = 1;
        }
      } catch (error) {
        console.error('Failed to fetch users:', error);
        this.users = [];
        this.totalPages = 1;
      }
    },
    async fetchAllUsers() {
      let allUsers = [];
      let pageNumber = 1;
      let pageSize = 100; 
      let totalPages = 1;

      try {
        while (pageNumber <= totalPages) {
          const response = await axios.get(`https://localhost:44345/api/users?pageNumber=${pageNumber}&pageSize=${pageSize}`);
          if (response.data) {
            allUsers = allUsers.concat(response.data.users);
            totalPages = response.data.totalPages;
            pageNumber++;
          } else {
            break;
          }
        }
        return allUsers;
      } catch (error) {
        console.error('Failed to fetch users:', error);
        return [];
      }
    },
    exportToExcel() {
      this.fetchAllUsers().then(users => {
        // Tạo một workbook mới
        const wb = XLSX.utils.book_new();

        // Chuyển đổi dữ liệu người dùng thành một worksheet
        const ws = XLSX.utils.json_to_sheet(users.map(user => ({
          Name: user.name,
          Email: user.email,
          Password: this.truncatedPassword(user.passwordHash),
          'Update Date': new Date(user.updatedAt).toLocaleString(),
          'Create Date': new Date(user.createdAt).toLocaleString()
        })));

        // Thêm worksheet vào workbook
        XLSX.utils.book_append_sheet(wb, ws, 'Users');

        // Xuất file Excel
        XLSX.writeFile(wb, 'Users.xlsx');
      }).catch(error => {
        console.error('Error exporting to Excel:', error);
      });
    },
    addNewUser() {
      // Handle adding a new user here
    },
    viewUser(userId) {
      this.selectedUser = this.users.find(user => user.userID === userId);
      this.showUserDetail = true;
    },
    closeUserDetail() {
      this.showUserDetail = false;
      this.selectedUser = {};
    },
    async deleteUser(userId) {
      const confirmed = confirm('Are you sure you want to delete this user?');
      if (!confirmed) return;

      try {
        const response = await axios.delete(`https://localhost:44345/api/users/${userId}`);
        if (response.status === 204) {
          // Xóa thành công, cập nhật danh sách người dùng
          this.users = this.users.filter(user => user.userID !== userId);
          console.log(`User with ID ${userId} deleted successfully.`);
        } else {
          console.error(`Failed to delete user with ID ${userId}`);
        }
      } catch (error) {
        console.error('Error deleting user:', error);
      }
    },
    truncatedPassword(password) {
      return password.length > 15 ? `${password.substring(0, 15)}...` : password;
    },
    prevPage() {
      if (this.currentPage > 1) {
        this.currentPage--;
        this.fetchUsers(this.currentPage, this.usersPerPage);
      }
    },
    nextPage() {
      if (this.currentPage < this.totalPages) {
        this.currentPage++;
        this.fetchUsers(this.currentPage, this.usersPerPage);
      }
    },
    goToPage(page) {
      if (page === '...') return;
      if (page >= 1 && page <= this.totalPages) {
        this.currentPage = page;
        this.fetchUsers(this.currentPage, this.usersPerPage);
      }
    }
  },
  mounted() {
    this.userName = localStorage.getItem('userName') || 'Admin';
    console.log('User Name:', this.userName);
    this.fetchUsers(this.currentPage, this.usersPerPage);
  }
};


</script>

<style scoped>
.container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background: linear-gradient(to bottom, #0081A7, #00AFB9);
  padding: 20px;
}

.user-manager {
  width: 100%;
  max-width: 1200px;
  border: 1px solid #ccc;
  padding: 40px;
  background-color: #fff;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
  border-radius: 12px;
  position: relative;
}

.modal-background {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.7);
  z-index: 999;
}

.user-detail-modal {
  position: fixed;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 500px;
  max-width: 90%;
  background-color: #fff;
  padding: 30px;
  box-shadow: 0 0 25px rgba(0, 0, 0, 0.4);
  border-radius: 12px;
  z-index: 1000;
  text-align: left; /* Chỉnh text-align sang left cho gọn gàng hơn */
}

.user-detail-modal h3 {
  margin-bottom: 20px;
  font-size: 24px;
  color: #333;
}

.user-detail-modal p {
  margin: 10px 0;
  font-size: 16px;
  color: #555;
}

.user-detail-modal .close-button {
  position: absolute;
  top: 10px;
  right: 10px;
  width: 30px;
  height: 30px;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: #299BE4;
  color: #fff;
  border: none;
  border-radius: 50%;
  cursor: pointer;
  font-size: 20px;
  font-weight: 600;
  transition: background-color 0.3s ease;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);
}

.user-detail-modal .close-button:hover {
  background-color: #268fd1;
}

/* Additional CSS for better visuals */
.user-detail-modal p strong {
  color: #000;
}

.user-detail-modal h3 {
  border-bottom: 2px solid #299BE4;
  padding-bottom: 10px;
  margin-bottom: 20px;
  text-align: center;
  font-size: 26px;
  color: #333;
}

.user-detail-modal p {
  margin: 12px 0;
  font-size: 18px;
  line-height: 1.6;
  color: #444;
}

.user-detail-modal p strong {
  display: inline-block;
  width: 120px;
  font-weight: 700;
}

.user-detail-modal p:last-child {
  margin-bottom: 0;
}

.user-detail-modal .close-button {
  position: absolute;
  top: 10px;
  right: 10px;
  width: 30px;
  height: 30px;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: #299BE4;
  color: #fff;
  border: none;
  border-radius: 50%;
  cursor: pointer;
  font-size: 18px;
  transition: background-color 0.3s ease;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);
}

.user-detail-modal .close-button:hover {
  background-color: #268fd1;
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

.logout-button {
  padding: 8px 16px;
  background-color: #299BE4;
  color: #fff;
  border: none;
  border-radius: 2px;
  cursor: pointer;
  font-size: 15px;
  font-weight: 600;
}

.logout-button:hover {
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
  margin-right: 12px;
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
  font-size: 14px;
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
  font-size: 14px;
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
  margin: 0 5px;
  background-color: #007bff;
  color: white;
  border: none;
  padding: 8px 10px;
  cursor: pointer;
  border-radius: 4px;
}

.pagination button:disabled {
  background-color: #ccc;
}

.pagination span {
  margin: 0 5px;
  padding: 8px 12px;
  cursor: pointer;
  border-radius: 4px;
  transition: background-color 0.3s;
}

.pagination .active {
  font-weight: bold;
  background-color: #dadcdd;
  color: rgb(0, 0, 0);
}

.pagination span:hover:not(.active) {
  background-color: #f0f0f0;
}

.no-data {
  text-align: center;
  font-weight: bold;
  color: #ff0000;
  /* màu đỏ */
}
</style>
