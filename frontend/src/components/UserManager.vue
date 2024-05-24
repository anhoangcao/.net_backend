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
      <h2 class="h2-user">User Management</h2>
      <div class="actions">
        <button @click="exportToExcel" class="export-add-button">
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 384 512" class="excel-icon">
            <path
              d="M64 0C28.7 0 0 28.7 0 64V448c0 35.3 28.7 64 64 64H320c35.3 0 64-28.7 64-64V160H256c-17.7 0-32-14.3-32-32V0H64zM256 0V128H384L256 0zM155.7 250.2L192 302.1l36.3-51.9c7.6-10.9 22.6-13.5 33.4-5.9s13.5 22.6 5.9 33.4L221.3 344l46.4 66.2c7.6 10.9 5 25.8-5.9 33.4s-25.8 5-33.4-5.9L192 385.8l-36.3 51.9c-7.6 10.9-22.6 13.5-33.4 5.9s-13.5-22.6-5.9-33.4L162.7 344l-46.4-66.2c-7.6-10.9-5-25.8 5.9-33.4s25.8-5 33.4 5.9z" />
          </svg>
          Export to Excel
        </button>
        <button @click="openAddUserModal" class="export-add-button">
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
              <button @click="viewUser(index)" class="action-button view-icon">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 576 512">
                  <path
                    d="M288 32c-80.8 0-145.5 36.8-192.6 80.6C48.6 156 17.3 208 2.5 243.7c-3.3 7.9-3.3 16.7 0 24.6C17.3 304 48.6 356 95.4 399.4C142.5 443.2 207.2 480 288 480s145.5-36.8 192.6-80.6c46.8-43.5 78.1-95.4 93-131.1c3.3-7.9 3.3-16.7 0-24.6c-14.9-35.7-46.2-87.7-93-131.1C433.5 68.8 368.8 32 288 32zM144 256a144 144 0 1 1 288 0 144 144 0 1 1 -288 0zm144-64c0 35.3-28.7 64-64 64c-7.1 0-13.9-1.2-20.3-3.3c-5.5-1.8-11.9 1.6-11.7 7.4c.3 6.9 1.3 13.8 3.2 20.7c13.7 51.2 66.4 81.6 117.6 67.9s81.6-66.4 67.9-117.6c-11.1-41.5-47.8-69.4-88.6-71.1c-5.8-.2-9.2 6.1-7.4 11.7c2.1 6.4 3.3 13.2 3.3 20.3z" />
                </svg>
              </button>
              <button @click="editUser(user.userID, index)" class="action-button edit-icon">
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
      <div class="pagination" v-if="filteredUsers.length > 0">
        <button @click="prevPage" :disabled="currentPage === 1" class="pagination-info">
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 320 512" class="icon-left">
            <path
              d="M9.4 233.4c-12.5 12.5-12.5 32.8 0 45.3l192 192c12.5 12.5 32.8 12.5 45.3 0s12.5-32.8 0-45.3L77.3 256 246.6 86.6c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5 0-45.3l-192 192z" />
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
      <p><strong>STT:</strong> {{ selectedIndex + 1 + (currentPage - 1) * usersPerPage }}</p>
      <p><strong>Name:</strong> {{ selectedUser.name }}</p>
      <p><strong>Email:</strong> {{ selectedUser.email }}</p>
      <p><strong>Update Date:</strong> {{ new Date(selectedUser.updatedAt).toLocaleString() }}</p>
      <p><strong>Create Date:</strong> {{ new Date(selectedUser.createdAt).toLocaleString() }}</p>
      <button @click="closeUserDetail" class="close-button">x</button>
    </div>
    <!-- User Detail Modal -->

    <!-- Modal edit người dùng -->
    <div v-if="showUserDetail" class="user-detail-modal">
      <h3>{{ isEditing ? 'Edit User' : 'User Details' }}</h3>
      <p><strong>STT:</strong> {{ selectedIndex + 1 + (currentPage - 1) * usersPerPage }}</p>
      <p><strong>Name:</strong>
        <input v-if="isEditing" v-model="selectedUser.name" type="text" />
        <span v-else>{{ selectedUser.name }}</span>
      </p>
      <p><strong>Email:</strong>
        <input v-if="isEditing" v-model="selectedUser.email" type="email" />
        <span v-else>{{ selectedUser.email }}</span>
      </p>
      <p><strong>Update Date:</strong> {{ new Date(selectedUser.updatedAt).toLocaleString() }}</p>
      <p><strong>Create Date:</strong> {{ new Date(selectedUser.createdAt).toLocaleString() }}</p>
      <div v-if="isEditing" class="modal-actions">
        <button @click="saveUser" class="save-button">Save</button>
        <button @click="closeUserDetail" class="close-button">x</button>
      </div>
      <button v-else @click="closeUserDetail" class="close-button">x</button>
    </div>
    <!-- Modal edit người dùng -->

    <!-- Modal thêm người dùng mới -->
    <div v-if="showAddUserModal" class="modal-background"></div>
    <div v-if="showAddUserModal" class="user-detail-modal">
      <h3>Add New User</h3>
      <p><strong>Name:</strong> <input v-model="newUser.name" type="text" /></p>
      <p><strong>Email:</strong> <input v-model="newUser.email" type="email" /></p>
      <p><strong>Password:</strong> <input v-model="newUser.password" type="password" /></p>
      <div class="modal-actions">
        <button @click="saveNewUser" class="save-button">Save</button>
        <button @click="closeAddUserModal" class="close-button">x</button>
      </div>
    </div>
    <!-- Modal thêm người dùng mới -->

  </div>
</template>

<script>
import * as XLSX from 'xlsx';
import axios from 'axios';
import bcrypt from 'bcryptjs';

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
      showAddUserModal: false,
      selectedUser: {},
      selectedIndex: 0,
      isEditing: false,
      newUser: { name: '', email: '', password: '' }
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
        console.log('Fetched Users:', users);

        const currentPage = 1;
        const usersPerPage = 100;

        const wb = XLSX.utils.book_new();
        const ws = XLSX.utils.json_to_sheet(users.map((user, index) => ({
          STT: index + 1 + (currentPage - 1) * usersPerPage,
          Name: user.name,
          Email: user.email,
          Password: this.truncatedPassword(user.passwordHash),
          'Update Date': new Date(user.updatedAt).toLocaleString(),
          'Create Date': new Date(user.createdAt).toLocaleString()
        })));

        XLSX.utils.book_append_sheet(wb, ws, 'Users');
        XLSX.writeFile(wb, 'Users.xlsx');
      }).catch(error => {
        console.error('Error exporting to Excel:', error);
      });
    },
    openAddUserModal() {
      this.showAddUserModal = true;
    },
    closeAddUserModal() {
      this.showAddUserModal = false;
      this.newUser = { name: '', email: '', password: '' };
    },
    async saveNewUser() {
      try {
        const hashedPassword = await bcrypt.hash(this.newUser.password, 10);
        const response = await axios.post(`https://localhost:44345/api/users`, {
          name: this.newUser.name,
          email: this.newUser.email,
          passwordHash: hashedPassword
        });
        if (response.status === 201) {
          this.fetchUsers(this.currentPage, this.usersPerPage);
          this.closeAddUserModal();
        } else {
          console.error('Failed to add new user');
        }
      } catch (error) {
        console.error('Error adding new user:', error.response ? error.response.data : error.message);
      }
    },
    viewUser(index) {
      this.selectedUser = this.filteredUsers[index];
      this.selectedIndex = index;
      this.showUserDetail = true;
    },
    closeUserDetail() {
      this.showUserDetail = false;
      this.selectedUser = {};
      this.isEditing = false;
      this.selectedIndex = 0;
    },
    editUser(userId, index) {
      this.selectedUser = this.users.find(user => user.userID === userId);
      this.selectedIndex = index;
      this.showUserDetail = true;
      this.isEditing = true;
    },
    async saveUser() {
      try {
        this.selectedUser.updatedAt = new Date().toISOString();
        const response = await axios.put(`https://localhost:44345/api/users/${this.selectedUser.userID}`, this.selectedUser);
        if (response.status === 200) {
          this.fetchUsers(this.currentPage, this.usersPerPage);
          this.closeUserDetail();
        } else {
          console.error(`Failed to update user with ID ${this.selectedUser.userID}`);
        }
      } catch (error) {
        console.error('Error saving user:', error);
      }
    },
    async deleteUser(userId) {
      const confirmed = confirm('Are you sure you want to delete this user?');
      if (!confirmed) return;

      try {
        const response = await axios.delete(`https://localhost:44345/api/users/${userId}`);
        if (response.status === 204) {
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
      return password.length > 20 ? `${password.substring(0, 20)}...` : password;
    },
    prevPage() {
      if (this.currentPage > 1 && this.filteredUsers.length > 0) {
        this.currentPage--;
        this.fetchUsers(this.currentPage, this.usersPerPage);
      }
    },
    nextPage() {
      if (this.currentPage < this.totalPages && this.filteredUsers.length > 0) {
        this.currentPage++;
        this.fetchUsers(this.currentPage, this.usersPerPage);
      }
    },
    goToPage(page) {
      if (page === '...' || this.filteredUsers.length === 0) return;
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

<style src="@/assets/style.css"></style>