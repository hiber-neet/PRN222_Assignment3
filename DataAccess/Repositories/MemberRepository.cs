using Asm03Solution.DataAccess.Models;
using Asm03Solution.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class MemberRepository : GenericRepository<Member>
    {
        public MemberRepository(ECommerceContext context) : base(context)
        {
        }

        public async Task<Member> LogIn(string email, string password)
        {
            var result = await _context.Members.FirstOrDefaultAsync(u =>
             u.Email == email &&
             u.Password == password);
            return result;
        }

        public async Task<List<Member>> GetAllUser()
        {
            return await _context.Members.ToListAsync();
        }

        public async Task<Member> GetMemberByEmail(string email)
        {
            Console.WriteLine("Email is get: " + email);
            var result = await _context.Members
      .AsNoTracking() // Đảm bảo không bị tracking entity
      .FirstOrDefaultAsync(u => u.Email == email);

            Console.WriteLine(result != null ? "Đã tìm thấy user!" : "Không tìm thấy user!"); // Log kết quả
            return result;
        }

        public async Task<Member> GetByIdAsync(int id)
        {
            return await _context.Members.Include(f => f.Orders).FirstOrDefaultAsync(b => b.MemberId == id);
        }

        public async Task<bool> UpdateMemberProfile(Member member)
        {
            var result = await UpdateAsync(member);
            return result > 0;
        }


    }
}
