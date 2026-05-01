# ShareBill

A scalable expense sharing API built with **ASP.NET Core**, designed to help groups of friends manage and settle shared expenses efficiently.

---

## 📖 About the Project

ShareBill provides the backend infrastructure to track shared costs and calculate who owes what. The core engine is designed to minimize the number of transactions needed to settle debts within a group. It is planned to be self hostable and scalable.

### Architecture & Approach

The project focuses on performance and clean architecture:
- **Data Access:** Uses **Dapper** and raw SQL.
- **Infrastructure:** Leverages **Supabase** to handle PostgreSQL database hosting and  Auth for secure user authentication.
- **Debt Optimization:** Implements a greedy algorithm to calculate the most efficient way to settle balances, ensuring users make the fewest possible transfers to clear their debts.

---

## 👨‍💻 Status

This project is currently under active development.

## 🚀 Tech Stack

- ⚙️ Backend: ASP.NET Core Web API (.NET 8)
- 🗄️ Database: PostgreSQL (Supabase)
- 🔐 Authentication: Auth (via Supabase)
- 🧠 Data Access: Dapper + raw SQL
- 📱 Frontend (planned): .NET MAUI / Android
- ☁️ Hosting: VPS
- 🔄 CI/CD: GitHub Actions (planned)

---

## ✨ Planned Features

- User authentication via Google
- Create and manage groups
- Add shared expenses
- Split expenses between participants (equally or by specific amounts)
- Track running balances between users
- Calculate the optimized settlement plan (who owes what to whom)

---

## 📌 Main Goals

- Backend architecture design (Clean / N-Tier architecture)
- Database design (relational modeling)
- REST API development with ASP.NET Core
- Debt settlement optimization algorithm (greedy approach)
- Deployment on Linux VPS
- CI/CD pipelines using GitHub Actions
- Mobile application development (MAUI / Android)

---
