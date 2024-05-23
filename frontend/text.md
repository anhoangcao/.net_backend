async deleteUser(userId) {
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




  // DELETE: api/Users/5
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteUser(int id)
{
    var user = await _context.Users.FindAsync(id);
    if (user == null)
    {
        _logger.LogWarning($"User with ID {id} not found.");
        return NotFound();
    }

    _context.Users.Remove(user);
    await _context.SaveChangesAsync();

    _logger.LogInformation($"User with ID {id} deleted successfully.");
    return NoContent();
}