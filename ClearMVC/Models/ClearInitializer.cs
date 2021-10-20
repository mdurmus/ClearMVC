using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace ClearMVC.Models
{
    public class ClearInitializer : CreateDatabaseIfNotExists<ClearModelContext>
    {
        protected override void Seed(ClearModelContext context)
        {
            int firmaId = InsertFirma(context);
            int masterUserTypeId = InsertMasterUserType(context);
            int managerUserTypeId = InsertManagerUserType(context);
            int personalUserTypeId = InsertPersonaUserType(context);
            int masterUserId = InsertMasterUsers(context, masterUserTypeId, firmaId);
            InsertMasterUserDetail(context, masterUserId);
            InsertProjectDetailsBuroTypes(context, firmaId);
            InsertProjectDetailsHalleTypes(context, firmaId);
            InsertCustomer(context, firmaId);
            int managerId = InsertManager(context, firmaId, managerUserTypeId);
            int personalId = InsertPersonal(context, firmaId, personalUserTypeId);
            InsertManagerDetail(context, managerId);
            InsertPersonalDetail(context, personalId);
            base.Seed(context);
        }
        private void InsertPersonalDetail(ClearModelContext context, int personalId)
        {
            UserDetails ud = new UserDetails
            {
                Area = "LU",
                City = "LU",
                CreateDate = DateTime.Now,
                GSM = "123",
                IsActive = true,
                Number = "123",
                Street = "Einbach",
                UserId = personalId
            };
            context.UserDetails.Add(ud);
            context.SaveChanges();
        }
        private void InsertManagerDetail(ClearModelContext context, int managerId)
        {
            UserDetails ud = new UserDetails
            {
                Area = "LU",
                City = "LU",
                CreateDate = DateTime.Now,
                GSM = "123",
                IsActive = true,
                Number = "123",
                Street = "Einbach",
                UserId = managerId
            };
            context.UserDetails.Add(ud);
            context.SaveChanges();
        }
        private int InsertPersonal(ClearModelContext context, int firmaId, int personalUserTypeId)
        {
            Users personal = new Users
            {
                UserTypesId = personalUserTypeId,
                CreateDate = DateTime.Now,
                Email = "personal@steril365.de",
                FirmaId = firmaId,
                IsActive = true,
                LastName = "Personal",
                Name = "Personal",
                Password = "123"
            };
            context.Users.Add(personal);
            context.SaveChanges();
            return personal.UsersId;
        }
        private int InsertManager(ClearModelContext context, int firmaId, int managerUserTypeId)
        {
            Users personal = new Users
            {
                UserTypesId = managerUserTypeId,
                CreateDate = DateTime.Now,
                Email = "manager@steril365.de",
                FirmaId = firmaId,
                IsActive = true,
                LastName = "Manager",
                Name = "Manager",
                Password = "123"
            };
            context.Users.Add(personal);
            context.SaveChanges();
            return personal.UsersId;
        }
        private void InsertCustomer(ClearModelContext context, int firmaId)
        {
            Customers customer = new Customers
            {
                Area = "LU",
                City = "LU",
                CreatedDate = DateTime.Now,
                Email = "kunden@steril365.de",
                FirmaId = firmaId,
                IsActive = true,
                Name = "Kunden",
                Number = "123",
                Password = "123",
                PostalCode = "67059",
                Phone = "123",
                Strasse = "Einbach"
            };
            context.Customers.Add(customer);
            context.SaveChanges();
        }
        private void InsertProjectDetailsHalleTypes(ClearModelContext context, int firmaId)
        {
            ProjectDetailsTypes projectDetailsTypes = new ProjectDetailsTypes
            {
                CreatedDate = DateTime.Now,
                IsActive = true,
                Type = "Halle",
                FirmaId = firmaId
            };
            context.ProjectDetailsTypes.Add(projectDetailsTypes);
            context.SaveChanges();
        }
        private void InsertProjectDetailsBuroTypes(ClearModelContext context, int firmaId)
        {
            ProjectDetailsTypes projectDetailsTypes = new ProjectDetailsTypes
            {
                CreatedDate = DateTime.Now,
                IsActive = true,
                Type = "Büro",
                FirmaId = firmaId

            };
            context.ProjectDetailsTypes.Add(projectDetailsTypes);
            context.SaveChanges();
        }
        private int InsertFirma(ClearModelContext context)
        {
            Firma firma = new Firma
            {
                Area = "RP",
                City = "LU",
                Email = "info@formr.de",
                Name = "Formr",
                Number = "1536",
                Phone = "1536",
                Street = "1536",
                VATCode = "1536",
                IsActive = true,
                CreatedDatetime = DateTime.Now
            };
            context.Firmas.Add(firma);
            context.SaveChanges();
            int firmaId = firma.Id;
            return firmaId;
        }
        private void InsertMasterUserDetail(ClearModelContext context, int masterUserId)
        {
            UserDetails userDetails = new UserDetails
            {
                UserId = masterUserId,
                CreateDate = DateTime.Now,
                IsActive = true,
                GSM = "123",
                Area = "LU",
                City = "LU",
                Number = "1",
                Street = "LU"
            };
            context.UserDetails.Add(userDetails);
            context.SaveChanges();
        }
        private int InsertMasterUsers(ClearModelContext context, int masterUserTypeId, int firmaId)
        {
            Users masterUser = new Users
            {
                CreateDate = DateTime.Now,
                IsActive = true,
                Password = "123",
                Email = "master@steril365.de",
                Name = "Master",
                LastName = "Master",
                UserTypesId = masterUserTypeId,
                FirmaId = firmaId
            };
            context.Users.Add(masterUser);
            context.SaveChanges();
            int masterUserId = masterUser.UsersId;
            return masterUserId;
        }
        private int InsertMasterUserType(ClearModelContext context)
        {
            UserTypes ut = new UserTypes
            {
                CreateDate = DateTime.Now,
                IsActive = true,
                Type = "Master"
            };
            context.UserTypes.Add(ut);
            context.SaveChanges();
            int masterUserTypeId = ut.UserTypesId;
            return masterUserTypeId;
        }
        private int InsertManagerUserType(ClearModelContext context)
        {
            UserTypes ut = new UserTypes
            {
                CreateDate = DateTime.Now,
                IsActive = true,
                Type = "Manager"
            };
            context.UserTypes.Add(ut);
            context.SaveChanges();
            return ut.UserTypesId;
        }
        private int InsertPersonaUserType(ClearModelContext context)
        {
            UserTypes ut = new UserTypes
            {
                CreateDate = DateTime.Now,
                IsActive = true,
                Type = "Personal"
            };
            context.UserTypes.Add(ut);
            context.SaveChanges();
            return ut.UserTypesId;
        }
    }
}
