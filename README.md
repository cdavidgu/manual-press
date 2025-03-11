# AR Asset Management System

## 📌 Project Overview
This mobile application provides augmented reality (AR) guidance for commissioning and maintaining a manual press and an interactive way to access to its technical documentation. Built using **Unity, C#, and Vuforia**, the app enhances user experience by integrating AR-based step-by-step instructions alongside traditional PDF documentation. The app operates **fully offline**, utilizing a **local SQL database** for data storage and management. The implementation of **MVC architecture** and SQL database ensures scalability, allowing future integrations for additional machines beyond the manual press, such as an engine or other industrial equipment.

## 🚀 Features
- **🛠️ Augmented Reality Interactive Guidance**   
    Step-by-step AR instructions for:

    - **Commissioning & Maintenance:** Interactive AR overlays guide users through key tasks, ensuring accuracy and efficiency.

    - **Manual Usage:** The AR-powered manual provides a hands-on, visual approach to understanding and operating the manual press.
    
    Users can choose between two AR modes:
  - **Marker-Based Tracking** – Point the camera at an image to activate AR guidance.
  - **Object Tracking** – Direct the camera at the real manual press for AR overlays.
- **🔍 Exploded 3D Model:**  View an interactive exploded view of the manual press, helping users understand its components and assembly.
- **📄 Document Viewer:** Easily access technical documents, including blueprints and datasheet.
- **📊 Local Database Storage:**  Uses SQL for storing and retrieving data.
- **🎨 UI/UX:** Designed for intuitive navigation and seamless AR interaction.
- **📴 Fully Offline:** No internet connection required for full functionality.


## 🏗️ Technology Stack

- **Unity** – Game engine used for development 
- **C#** -  Primary programming language.  
- **Vuforia** – AR framework for marker-based and object tracking.
- **SQL** – Local database for offline data management.

## 💻 Code & Architecture

The following architectural patterns were implemented to structured code implementation:

**Model-View-Controller (MVC) Pattern** – Followed to ensure data is stored and retrieved without affecting the UI layer or bussiness logics, and for future machine integrations, maintainability and scalability of the app.

**Singleton Pattern** – Used to manage the overall state of the app and its components.

**Entity-Component-System (ECS) pattern** - Used for maximizing Unity’s performance and its modular architecture design. 

## 📲 Installation & Setup
### Android
1. **Download the APK** ([here](https://is.gd/EM08kB)).
2. Install the APK on an Android device (enable "Unknown Sources" if needed).
3. Open the app and explore its features!

### iOS
(Currently not publicly available; it requires manual deployment via Xcode.)

## 🎮 How It Works
1. Launch the app, it will start with an introduction that explains its functionalities.
2. After the intro, the main menu appears, allowing the user to select between **Marker-Based Tracking** (scan this [image](https://is.gd/EM08kB)) or **Object Tracking** (scan the manual press itself).
3. Once an AR mode is selected and the camera recognizes the target (marker or machine), a Head-Up Display (HUD) appears with interactive buttons for:
    - Exploded 3D Model
    - Maintenance guide
    - Commissioning guide
    - Manual Usage
    - Document Viewer
3. Follow the AR-guided steps.
4. Interact with the Exploded 3D Model to analyze individual parts of the press.

## 🎯 Use Cases
- Industrial training for technicians.
- Asset Management.
- On-site machine maintenance support.
- Interactive manuals replacing traditional paper-based guides.

## 🛠️ Challenges & Solutions
#### Challenge: Accurate AR tracking on real-world objects.
**Solution:** Implemented high-quality object recognition using Vuforia’s advanced tracking algorithms.

#### Challenge: Balancing functionality with user experience.
**Solution:** Designed an intuitive UI with smooth navigation and seamless transitions between AR and standard views.

#### Challenge: Storing and retrieving data efficiently in an offline environment.
**Solution:** Implemented a local SQL database for structured and efficient data management. 

## 🔜 Future Improvements
- Cloud-based data storage to update machine information
- Support additional machines and equipment.


