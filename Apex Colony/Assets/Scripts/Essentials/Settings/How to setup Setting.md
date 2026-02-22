> Create an "SettingsSO" (scriptable object if haven't) ONLY 1 should exist
> In scene that use Settings add the manager and all "Setting_Type" to object (they are singleton).
> Assign the "SettingsSO" to "SettingsManager".
> Assign UI element to Setting_Type.