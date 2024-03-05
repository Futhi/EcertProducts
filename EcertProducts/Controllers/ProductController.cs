using BusinessLogic.Common;
using BusinessLogic.Models;
using BusinessLogic.Services;
using Dasync.Collections;
using DataAccess.ApplicationContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Collections.Generic;
//using DocumentFormat.OpenXml.Drawing.Charts;
//using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
//using DocumentFormat.OpenXml.Spreadsheet;
//using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

namespace EcertProducts.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {

        private readonly IProductService _service;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ApplicationDBContext _dbContext;

        public string FileNameOnServer { get; set; }
        public long FileContentLength { get; set; }
        public string FileContentType { get; set; }

        public ProductController(ApplicationDBContext context, IProductService service, ICategoryService _catService, IWebHostEnvironment webHostEnvironment)
        {         
            _service = service;
            _categoryService = _catService;
            _hostEnvironment = webHostEnvironment;

            FileNameOnServer = string.Empty;
            FileContentLength = 0;
            FileContentType = string.Empty;
            _dbContext= context;
        }


        // GET: ProductController
        public async Task<ActionResult> Index(int pagenumber=1)
        {
            List<Product> data =await _service.GetAllProducts();
            var pagedData =  PaginatedList<Product>.CreateAsync(data, pagenumber,5);

            return View(pagedData);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Creates
        public ActionResult Create()
        {
            ProductModel model = new ProductModel();
            ProductModel viewmodel = new ProductModel();
            var uploadmodel = UploadModel(model);
            viewmodel.AvailableCategories = uploadmodel.Result.AvailableCategories;
            viewmodel.ProductCode= uploadmodel.Result.ProductCode;
            return View(viewmodel);
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductModel model)
        {
            try
            {  
                if (!ModelState.IsValid)
                 return View(model);

                string filename = "";
                if (model.picture != null)
                {
                    string uploadfolder = Path.Combine(_hostEnvironment.WebRootPath,"images");
                    filename = model.picture.FileName;
                    string filepath = Path.Combine(uploadfolder,filename);
                    model.picture.CopyTo(new FileStream(filepath, FileMode.Create));
                }
                
                await  _service.CreateProduct(model,filename);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }

        // GET: ProductController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {

            Task<Product> producDetails;
             producDetails = _service.GetProductById(id);
            List<Category> categories = await _service.GetAllAvailableCategories();

            ProductModel model = new ProductModel {

                Name = producDetails.Result.Name,
                ProductCode = producDetails.Result.ProductCode,
                Description = producDetails.Result.Description,
                CategoryName = producDetails.Result.CategoryName,
                Price = producDetails.Result.Price,
                AvailableCategories = categories           
            };

            if (producDetails == null)
                return View("Not Found");


            return View(model);


          
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductModel model)
        {
            try
            {
                 string filename = "";
                if (model.picture != null)
                {
                    string uploadfolder = Path.Combine(_hostEnvironment.WebRootPath,"images");
                    filename = model.picture.FileName;
                    string filepath = Path.Combine(uploadfolder,filename);
                    model.picture.CopyTo(new FileStream(filepath, FileMode.Create));
                }
               
                await _service.UpdateProduct(model,filename);
             return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View(ex);
            }

            
        }

        // GET: ProductController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var productDetails =await  _service.GetProductById(id);

            if (productDetails == null)
                return View("Not Found");

            return View(productDetails);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id, IFormCollection collection)
        {
            try
            {
                await _service.DeleteProduct(id);
               return RedirectToAction(nameof(Index));
            }
            catch(Exception)
            {
                return View();
            }
            
        }

        //This UploadModel method takes all the saved products and generates a spreadsheet based on that data
        private async Task<ProductModel> UploadModel(ProductModel model)
        {

            List<Category> categories =await  _service.GetAllAvailableCategories();

            model.AvailableCategories = categories;

            var year = DateTime.Now.Year.ToString();
            var month = DateTime.Now.Month.ToString();
            var count_id =await _service.GetAllProducts();
            var productCode = $"{year}{month}-{count_id.Count+1}";
            model.ProductCode = productCode;

            return  model;
        }

        public async Task  DownloadExcel()
        {
            var productData = await _service.GetAllProducts();

            if (productData.Count > 0)
            {
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("ProductsData");

                    // Add headers
                    worksheet.Cells["A1"].Value = "Id";
                    worksheet.Cells["B1"].Value = "Name";
                    worksheet.Cells["C1"].Value = "ProductCode";
                    worksheet.Cells["D1"].Value = "Description";
                    worksheet.Cells["E1"].Value = "CategoryName";
                    worksheet.Cells["F1"].Value = "Price";
                    worksheet.Cells["G1"].Value = "Image";
                    int row = 2; // Starting from the second row for data
                    List<Product> products = new List<Product>();
                    products = productData;
                    foreach (var item in productData)
                    {
                        worksheet.Cells[$"A{row}"].Value = item.Id;
                        worksheet.Cells[$"B{row}"].Value = item.Name;
                        worksheet.Cells[$"C{row}"].Value = item.ProductCode;
                        worksheet.Cells[$"D{row}"].Value = item.Description;
                        worksheet.Cells[$"E{row}"].Value = item.CategoryName;
                        worksheet.Cells[$"F{row}"].Value = item.Price;
                        worksheet.Cells[$"G{row}"].Value = item.Image;

                        row++;
                    }

                    // Auto-fit columns for better readability
                    worksheet.Cells.AutoFitColumns();

                    // Save the Excel package to a stream
                    var stream = new MemoryStream(package.GetAsByteArray());

                    // Set the position to the beginning of the stream
                    stream.Position = 0;

                    // Send the Excel file as a response
                    Response.Clear();
                    Response.Headers.Add("Content-Disposition", "attachment; filename=Products.xlsx");//name of the document
                    Response.Headers.Add("Content-Length", stream.Length.ToString());
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    stream.CopyTo(Response.Body);
                }
            }
          
        }

        [HttpPost]
        public async  Task<IActionResult> UploadFile(IFormFile fileToUpload)
        {
           
            if (fileToUpload == null || fileToUpload.Length == 0)
            {
                // Handle invalid or missing file
                return BadRequest("Invalid or missing file");
            }

            using (var stream = new MemoryStream())
            {
                await fileToUpload.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    // Assuming there is only one worksheet in the Excel file
                    var worksheet = package.Workbook.Worksheets.First();

                    // Process the data in the worksheet as needed
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        // Assuming the data is in the same format as the previous example
                        //  var id = worksheet.Cells[row, 1].GetValue<int>();
                        var name = worksheet.Cells[row, 2].GetValue<string>();
                        var productCode = worksheet.Cells[row, 3].GetValue<string>();
                        var description = worksheet.Cells[row, 4].GetValue<string>();
                        var categoryName = worksheet.Cells[row, 5].GetValue<string>();
                        var price = worksheet.Cells[row, 6].GetValue<string>();
                        var imageName = worksheet.Cells[row, 7].GetValue<string>();
                        // Use the extracted data as needed (e.g., save to the database)

                        

                        ProductModel model = new ProductModel
                        {

                            Name = name,
                            ProductCode = productCode,
                            Description = description,
                            CategoryName = categoryName,
                            Price = decimal.Parse(price)

                        };

                       
                        if (model.CategoryName == "")
                        {
                            continue;
                        }
                        else
                        {
                      var   category = _dbContext.Categorys.FirstOrDefault(p => p.Name == model.CategoryName);
                            if (category is not null)
                            {
                                model.CategoryId = category.Id;
                            }

                            await _service.CreateProduct(model, imageName);
                        }
                         
                       

                        // Note: Adjust the data types and column indexes based on the actual Excel file format.
                    }
                }
            }

            // Handle successful file upload
            return Ok("File uploaded successfully");
        }
    }
}
