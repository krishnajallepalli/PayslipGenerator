﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace PaySlipGenerator
{
    public class Employee : IEquatable<object>
    {
        public Employee(string firstName, string lastName, double annualSalary, int superRate)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.AnnualSalary = annualSalary;
            this.SuperRate = superRate;

            this.Payslips = new List<PaySlip>();
        }

        public Employee(string firstName, string lastName, double annualSalary, int superRate, List<PaySlip> payslips)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.AnnualSalary = annualSalary;
            this.SuperRate = superRate;
            this.Payslips = payslips;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double AnnualSalary { get; set; }
        public int SuperRate { get; set; }
        public List<PaySlip> Payslips { get; set; }

        public override bool Equals(object other)
        {
            if( !(other is Employee) )
            {
                return false;
            }

            Employee emp = (Employee)other;

            if ( !FirstName.Equals(emp.FirstName))
            {
                return false;
            }

            if ( !(LastName.Equals(emp.LastName)) )
            {
                return false;
            }

            if ( !(AnnualSalary.Equals(emp.AnnualSalary)))
            {
                return false;
            }

            if ( !(SuperRate.Equals(emp.SuperRate)))
            {
                return false;
            }

            if (this.Payslips.Except(emp.Payslips).Count() > 0 || emp.Payslips.Except(this.Payslips).Count() > 0)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, LastName);
        }
    }
}
