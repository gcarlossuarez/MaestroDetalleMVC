using MaestroDetalleMVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MaestroDetalleMVC.Models;

namespace MaestroDetalleMVC.Controllers
{
    public class MaestroDetalleController : Controller
    {
        // GET: MaestroDetalle
        //public ActionResult Index()
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(VentaViewModel model)
        {
            
            using (MaestroDetalleMVCEntities db = new MaestroDetalleMVCEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        venta oVenta = new venta
                        {
                            fecha = DateTime.Now,
                            cliente = model.Cliente
                        };

                        db.venta.Add(oVenta);

                        db.SaveChanges();

                        foreach (var oC in model.conceptos)
                        {
                            concepto oconcepto = new concepto()
                            {
                                cantidad = oC.Cantidad,
                                nombre = oC.Nombre,
                                precioUnitario = oC.PrecioUnitario,
                                total = oC.Cantidad * oC.PrecioUnitario,
                                idVenta = oVenta.id
                            };
                            db.concepto.Add(oconcepto);

                        }

                        db.SaveChanges();

                        dbContextTransaction.Commit();

                        ViewBag.Message = "Registro insertado";
                        // Si no se pone nada en "View()", trata de retornar una vista con el mismo nombre del método. Como no hay una vista
                        // llamada "Add", da error (habría que tener u ".cshtml" llamado "Add.cshtml". Otra solución, sería cambiar el nombre de
                        // "Index.cshtml" por "Add.cshtml" y el contructor "Index()", cambiarlo por "Add()", en esta clase (que es lo que, al fi-
                        // nal, se hizo).
                        //return View();
                        //return (model);
                        //return RedirectToAction("Index");
                        return View();
                    }
                    catch(Exception ex)
                    {
                        dbContextTransaction.Rollback();

                        ViewBag.Message = ex.Message.ToString();
                        return View(model);
                    }
                        
                }
                        
            }
            
        }
    }
}