# ProspectLabApp

I created a shopping list app, a powerful ASP.NET web application built with Razor Pages. It works by processing PDF leaflets, converting the text into products through a series of filtering processes. The end result is a neatly organized, digital shopping list derived from a traditional leaflet.

Technologies used:
* C# with Razor Pages for seamless web development.
* HTML, CSS and Bootstrap for a visually appealing and user-friendly interface
* JavaScript for interactive and dynamic functionalities.
* Entity Framework for efficient data management and SQLExpress integration.

# How to run
1. 
Install the following:
https://visualstudio.microsoft.com/vs/community/
https://www.microsoft.com/en-us/download/details.aspx?id=101064
2. 
Clone the repository
3. 

Main Page
=
<p align="center">
    <img src="https://github.com/bclaura/ProspectLabApp/assets/124773748/7a009c6a-6653-4490-85c2-2c29018be728" width="500px">
</p>

## Main Page Image Banner

Those banners are meant for a future update of the app. This will allow you to sort products by category.

<p align="center">
    <img src="https://github.com/bclaura/ProspectLabApp/assets/124773748/a61ab447-5fb5-4621-9551-5bbf0ca492ee" width="500px">
</p>

Offers Page
=
I utilized JqGrid to effectively display products from the database. With a simple click on the green button next to each product, the product's ID is stored in a cookie. This ID is then used to populate the shopping list.
<p align="center">
    <img src="https://github.com/bclaura/ProspectLabApp/assets/124773748/1e58a8d6-a31c-4fae-b183-06a4a57c39ec" width="500px">
</p>

## JqGrid Pagination
Product navigation is more manageable with the JqGrid pagination feature. It offers the flexibility to customize the number of products displayed per page, allowing you to change the interface to your own preference.
<p align="center">
    <img src="https://github.com/bclaura/ProspectLabApp/assets/124773748/1981eeed-a566-4d4c-8092-fd773185b119" width="500px">
</p>

Shopping List Page
=
<p align="center">
    <img src="https://github.com/bclaura/ProspectLabApp/assets/124773748/8871e764-9af0-49af-a4f2-a3e87d74a5a1" width="500px">
</p>

## Cookie Detail
Products added to the Offers page are stored in a cookie with a default quantity of 1. This cookie, which contains the unique product GUID, has a lifespan of 2 hours. After this period the cookie is deleted for optimal performance.
<p align="center">
    <img src="https://github.com/bclaura/ProspectLabApp/assets/124773748/5908f27a-da70-4544-96f6-943125ba10ee" width="500px">
</p>

## Add and remove buttons
For each product a "+" button is visible that allows you to increase the quantity. If a product's quantity is 2 or more, a "-" button will also appear, enabling you to decrease the quantity. Once the quantity is reduced back to 1, the "-" button will automatically disappear, maintaining a clean and user-friendly interface.
<p align="center">
    <img src="https://github.com/bclaura/ProspectLabApp/assets/124773748/7dc7f9c6-f643-45df-a14d-0e1f89e60d7d">
</p>

## Delete button
By simply clicking anywhere on a product row, the row will be visually 'cut out', and a trash button will appear alongside the other buttons. This indicates the product is marked for deletion but has not been removed yet. Clicking the row again will revert it back to its normal state. 'Cut out' products are not affecting the total price, as this will only change when a product is completely deleted from the list.
<p align="center">
    <img src="https://github.com/bclaura/ProspectLabApp/assets/124773748/efe51d66-08e5-499b-93b3-4fac36cae18f" width="500px">
</p>

## Delete products button result
A bulk deletion is performed on all the cutout products with the "Sterge produsele selectate" button.
<p align="center">
    <img src="https://github.com/bclaura/ProspectLabApp/assets/124773748/7aa3dbab-52f3-45fc-a64f-c499b17d06c3" width="500px">
</p>

## Create a new list button result
The button "Creeaza o noua lista" clears the entire list and lets you begin anew.
<p align="center">
    <img src="https://github.com/bclaura/ProspectLabApp/assets/124773748/326accc6-c5b2-4857-b423-0997c77fd977">
</p>

Mobile View
=
<p align="center">
    <img src="https://github.com/bclaura/ProspectLabApp/assets/124773748/b2c2796f-1b03-48d7-a52b-66b2b373ccb1" width="250px">
</p>

<p align="center">
    <img src="https://github.com/bclaura/ProspectLabApp/assets/124773748/008194d2-2217-4e60-a658-0b28d758df5c" width="250px">
</p>

<p align="center">
    <img src="https://github.com/bclaura/ProspectLabApp/assets/124773748/2b8b7d8f-757b-4684-865a-4e09f964bdb4" width="250px">
</p>

<p align="center">
    <img src="https://github.com/bclaura/ProspectLabApp/assets/124773748/26b14954-39bd-4c0a-98bf-cbf1bff5680f" width="250px">
</p>
