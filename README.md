# 🎉 EventEase – Full Event Management System

![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET-Core-blue)
![EF Core](https://img.shields.io/badge/Entity%20Framework-Core-green)
![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-red)
![TailwindCSS](https://img.shields.io/badge/UI-TailwindCSS-38B2AC)
![License](https://img.shields.io/badge/License-Educational-lightgrey)

EventEase is a modern ASP.NET Core MVC web application for managing **events, venues, and bookings**.  

---

# 🌍 Overview

EventEase centralizes event management into one system by allowing users to:

- Manage venues and capacity
- Create categorized events
- Book events into venues
- Upload and store venue images in the cloud
- Filter bookings
- Enforce relational database integrity

The system demonstrates real-world architecture and scalability principles.

---

# 🏗 Architecture

EventEase follows the ASP.NET Core MVC pattern:


### Layers

- **Presentation Layer** → Razor Views (Tailwind CSS)
- **Application Layer** → Controllers
- **Data Layer** → Entity Framework Core
- **Persistence Layer** → SQL Server
- **Cloud Layer** → Azure Blob Storage

---

# 🚀 Features

## 🏟 Venue Management

- Create, edit, delete venues
- Upload venue images to Azure Blob Storage
- Track venue capacity
- Display availability status
- Prevent deletion if venue has active bookings
- Support large stadiums (e.g., 94,736 capacity)

---

## 📅 Event Management

- Create events with descriptions
- Categorize by Event Type:
  - Tech Conference
  - Business Workshop
  - Music Festival
  - Private Celebration
  - Community Expo
  - Sports Event
- Link events to venues
- Manage event scheduling

---

## 📝 Booking Management

- Create bookings between events and venues
- Filter by:
  - Event type
  - Date range
  - Availability
- Maintain foreign key integrity
- Prevent invalid updates

---

## 🏆 Sports & Stadium Support

Includes support for large-scale events:

- Big stadiums
- Sports event category
- Championship events
- High-capacity venues

---

# 🗄 Database Design

### Entities

### Venue
- VenueId (PK)
- VenueName
- Location
- Capacity
- ImageUrl
- IsAvailable

### Event
- EventId (PK)
- EventName
- Description
- EventDate
- EventTypeId (FK)

### EventType
- EventTypeId (PK)
- Name

### Booking
- BookingId (PK)
- BookingDate
- EventDate
- VenueId (FK)
- EventId (FK)

---

# 🔐 Data Integrity

✔ Foreign key enforcement  
✔ Prevent venue deletion if bookings exist  
✔ Model validation  
✔ Server-side validation  
✔ Client-side validation  
✔ Concurrency-safe updates  

---

# ☁ Azure Blob Storage Setup

Add your Azure credentials inside:

`appsettings.json`

```json
"AzureStorage": {
  "ConnectionString": "YOUR_CONNECTION_STRING",
  "ContainerName": "YOUR_CONTAINER_NAME"
}