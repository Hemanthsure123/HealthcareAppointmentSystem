# Healthcare Appointment Management System

## Architecture Overview

The system is built using a microservices architecture with the following components:

- **ClientApp**: ASP.NET Core MVC application for the patient portal, doctor dashboard, and admin panel.
- **ApiGateway**: Ocelot-based API Gateway for routing requests to microservices.
- **Microservices**:
  - **AppointmentService**: Manages appointment CRUD operations.
  - **UserService**: Handles user authentication, authorization, and profiles.
  - **TelemedicineService**: Manages telemedicine sessions and call links.
  - **AnalyticsService**: Provides analytics and reports.
  - **NotificationService**: Handles real-time notifications using SignalR.
- **Azure Health Data Services**: Centralized data store for healthcare data (FHIR and DICOM).
- **Azure Key Vault**: Stores secrets and encryption keys.
- **Azure Blob Storage**: Stores files like telemedicine recordings and reports.
- **Azure SQL**: Additional database if needed (though Azure Health Data Services may suffice).
- **SignalR Service**: For scaling real-time notifications.

## Architecture Diagram
![Architecture Diagram](https://raw.githubusercontent.com/HealthcareAppointmentSystem/healthcare-appointment-management-system/main/docs/architecture-diagram.png)


## Data Flow
- **ClientApp** communicates with **ApiGateway**.
- **ApiGateway** routes requests to the appropriate microservice.
- Microservices interact with **Azure Health Data Services** for data storage and retrieval.
- **NotificationService** uses **SignalR** for real-time updates.
- All services use **Azure Key Vault** for secure secret management.