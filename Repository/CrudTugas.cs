using Microsoft.EntityFrameworkCore;
using ToDoList.DataContext;
using ToDoList.DTO;
using ToDoList.Model;
using ToDoList.Interface;
using SQLitePCL;


namespace ToDoList.Repository{

    public class CrudTugas : ICrudTugas {

    private readonly Connection _context;
    

    public CrudTugas(Connection conetxt){
        _context = conetxt;
    
    }

    public async Task<int> Create (TugasDto tugasDto){
       
        var Category_Name = tugasDto.category_name;
        try{
            var Category_result = _context.category.Where(c => c.Id == Category_Name).FirstOrDefault();
            

            var tugas = new Tugas{
                Deskripsi = tugasDto.deskripsi,
                Category_ID = Category_result.Id,
                Category_FK = _context.category.Where(c => c.Id == Category_Name).FirstOrDefault(),
            };


            _context.Add(tugas);
            return await _context.SaveChangesAsync();
       }catch (ArgumentNullException e){
            throw new ArgumentNullException(String.Format("Object Not Found {0}", e));
       }
        
        
       
    }
    
   public async Task<List<Tugas>> GetAll(){
        return await _context.tugas.ToListAsync();
        
        }
    

    public async Task<Tugas> GetById(int id){
        var result = await _context.tugas.FirstOrDefaultAsync(t => t.Id == id);
         if (result == null){
                throw new ArgumentNullException(String.Format("Object Null : {0}" , result));
            }
        return result;
        
    }

    public async Task<int> Change_Status(int id, TugasSelesai status){
        var result = GetById(id);
        result.Result.Status = status;
        return await _context.SaveChangesAsync();
    }

    public async Task<int> Change_Category(string newCategory, int id){
        var result_tugas = GetById(id);
        var result_category = _context.category.FirstOrDefaultAsync(c => c.Name == newCategory);
        if (result_category == null){
                throw new ArgumentNullException(String.Format("Object Null : {0}" , result_category));
            }
        result_tugas.Result.Category_ID = result_category.Result.Id;
        return await _context.SaveChangesAsync();
    }

    public async Task<string> Category_name(int id){
        var result_tugas = _context.tugas.Where(t => t.Id == id).FirstOrDefaultAsync();
        return result_tugas.Result.Category_FK.Name;
    }
    
    }

}