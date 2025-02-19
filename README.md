# YahooFinanceScraper

## Overview

The StockScraper application is designed to scrape financial information from Yahoo! Finance for a list of stock tickers on a given date. The application follows Domain-Driven Design (DDD) principles to ensure a clean and maintainable architecture. 

### Company Details:

1. **Full Company Name**: The complete name of the company.
    
2. **Market Cap**: The market capitalization of the company.
    
3. **Year Founded**: The year the company was founded.
    
4. **Number of Employees**: The total number of employees working at the company.
    
5. **Headquarters: The city and state, country where the company's headquarters is located.
    

### Stock Price Information:

1. **Date and Time**: The specific date and time for which the data is being scraped and retrieved.
    
2. **Previous Close Price**: The closing price of the stock on the provided date.
    
3. **Open Price**: The opening price of the stock on the given date.


This data is then stored in a database (MSSQL) for further use.

## Domain-Driven Design (DDD)

Domain-Driven Design is a software development approach that focuses on modeling the core business domain and its logic. It emphasizes collaboration between technical and domain experts to create a shared understanding of the problem space. Key concepts in DDD include:

- **Entities**: Objects that have a distinct identity and lifecycle (e.g., `StockInfo`).
    
- **Value Objects**: Objects that are defined by their attributes and have no conceptual identity (e.g., `MarketCap`).
    
- **Repositories**: Mechanisms for retrieving and storing aggregates.
    
- **Services**: Stateless operations that fulfill domain tasks.
    

## Architecture

The application is structured into several projects, each with a specific responsibility:

1. **StockScraper.API**: The entry point of the application, handling HTTP requests and responses.
    
2. **StockScraper.Application**: Contains the application logic, including commands and queries.
    
3. **StockScraper.Contracts**: Defines the interfaces and contracts used across the application.
    
4. **StockScraper.Domain**: Encapsulates the core business logic and domain models.
    
5. **StockScraper.Infrastructure**: Manages data access, external services, and configurations.
    
6. **StockScraper.Presentation**: Handles the user interface and presentation logic.
    

## Getting Started

### Prerequisites

- Docker
    
- Docker Compose
    

### Running the Application

1. Clone the repository to your local machine.
    
2. Navigate to the root folder of the project.
    
3. Run the following command to build and start the application using Docker:
 ```bash
   docker-compose up --build
   ```


This command will build the Docker images and start the containers for the application and the database.

### Application Instructions

- The application will be accessible at `http://localhost:5001` and `http://localhost:5000`.
    
- Use the provided API endpoints to scrape and retrieve stock information.
    
- The scraped data will be stored in the database for future reference.
    
- History Tab will show all previous scrapings.
  

## Project Structure


src
├── StockScraper.API
├── StockScraper.Application
├── StockScraper.Contracts
├── StockScraper.Domain
├── StockScraper.Infrastructure
└── StockScraper.Presentation
tests

## Conclusion

The StockScraper application is a robust solution for scraping and storing financial data from Yahoo! Finance. By adhering to DDD principles and clean arhitecture, the application ensures a clear separation of concerns and maintainability. Follow the instructions above to get started with the application.