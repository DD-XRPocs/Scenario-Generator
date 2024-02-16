# Scenario Generator Facilitator Read Me

# Description
This package is a scenario generator facilitator designed to work within the Unity game development environment. It leverages an Excel spreadsheet to generate scenarios for use within your Unity projects. The package provides a streamlined process for importing scenario data from the Excel file and populating it within your Unity scenes.

# System Requirements
    •	Unity version 2022 or later.
    •	Install NuGet package Manager : https://github.com/GlitchEnzo/NuGetForUnity.git?path=/src/NuGetForUnity
    • From NuGet Package install : ExcelDataReader.3.6.0
    • Importe Timeline samples
    •	Excel file located in the Streaming Asset folder.
    •	Prefab for the scenario panel with specific requirements (TMP named "Title," TMP named "Description," and a disabled button named "Button").
    •	Install NuGet package - ExcelDataReader
    
# Usage Instructions
    •	Excel Template: Use the provided Excel template or create a new Excel file with the same structure as the template. Ensure that the structure matches the expected format for the scenario data.
    •	Panel Prefab: Use your own prefab for the scenario panel, ensuring that it meets the following requirements:
        o	Contains a Text Mesh Pro (TMP) component named "Title" for the scenario title.
        o	Contains a TMP component named "Description" for the scenario description.
        o	Contains a disabled button named "Button" for user interaction.

# Scenario Generator Setup:
    •	Drag and drop the "Scenario Container" prefab into your Unity scene.
    •	Attach your prefab for the scenario panel to the "Scenario Creator" script.
    •	Define the path location of your Excel file within the Streaming Asset folder.

# CSV Parser Configuration:
    •	Ensure that the "Set new scenario" option is enabled on the CSV Parser component.
    
# Generating Scenarios:
    •	In the Unity Editor, navigate to the Inspector window for the "Scenario Creator" script.
        o	Click on the "Generate Scenario" button to initiate the scenario generation process.
        
# Additional Notes
    •	This package provides a convenient tool for scenario generation within Unity projects.
    •	Customization options for the scenario panel prefab allow for seamless integration with existing project aesthetics.
    •	Ensure that the Excel file is correctly formatted and located in the designated Streaming Asset folder for proper functionality.
    
# Support
For any inquiries or issues related to this package, please contact Julie Morand
