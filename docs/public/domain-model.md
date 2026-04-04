# Domain Models

## Profile

### Purpose

Represents a person whose health information is managed in the system (self, family member, or dependent).

### Entity Type

- Core

### Fields

| Field Name | Type           | Nullable | Notes                                  |
| ---------- | -------------- | -------- | -------------------------------------- |
| Id         | int            | no       | primary key                            |
| Name       | string         | no       | max length 100                         |
| BirthDate  | DateOnly?      | yes      | optional                               |
| Sex        | int?           | yes      | enum mapping (e.g., Male/Female/Other) |
| Height     | decimal?       | yes      | meters or cm                           |
| CreatedAt  | DateTimeOffset | no       | timestamp of creation                  |
| UpdatedAt  | DateTimeOffset | no       | timestamp of last update               |

### Relationships

| Cardinality                                | Type | Optional/Required | On Delete Behavior | Notes                                    |
| ------------------------------------------ | ---- | ----------------- | ------------------ | ---------------------------------------- |
| 1 **Profile** → many **Doctors**           | 1:N  | required          | Cascade            | Each doctor belongs to a profile         |
| 1 **Profile** → many **Appointments**      | 1:N  | required          | Cascade            | Profile’s scheduled visits               |
| 1 **Profile** → many **Notes**             | 1:N  | required          | Cascade            | Freestanding or appointment-linked notes |
| 1 **Profile** → many **Documents**         | 1:N  | required          | Cascade            | Uploaded documents or scans              |
| 1 **Profile** → many **Vitals**            | 1:N  | optional          | Cascade            | Health metrics over time                 |
| 1 **Profile** → many **ProfileMedication** | 1:N  | optional          | Cascade            | Medication usage records                 |
| 1 **Profile** → many **SymptomLog**        | 1:N  | optional          | Cascade            | Symptom tracking                         |
| 1 **Profile** → many **ProfileCondition**  | 1:N  | optional          | Cascade            | Health conditions                        |

### Behavior / Business Rules (Entity Level)

- Cannot exist without a valid name.  
- All related entities must reference a profile.  
- Profile deletion cascades to dependent records (Doctors, Appointments, Notes, Documents, Vitals, ProfileMedication, SymptomLog, ProfileCondition).  
- Switching the active profile updates all visible data contextually.  

### Edge Cases

- No profiles exist → system requires profile creation before adding other entities.  
- Only one profile exists → automatically selected as active.  
- Deleting the active profile → system must require selecting or creating another profile before continuing.  

### Explicitly NOT included (for MVP)

- Profile-level permissions or access controls.  
- Profile sharing across users or devices.  
- Profile photos / avatars.  

## Doctor

### Purpose

Represents a medical provider associated with a profile for care management and appointments.

### Entity Type

- Core / Contact

### Fields

| Field Name | Type           | Nullable | Notes                    |
| ---------- | -------------- | -------- | ------------------------ |
| Id         | int            | no       | primary key              |
| ProfileId  | int            | no       | foreign key to Profile   |
| Name       | string         | no       | max length 150           |
| Specialty  | string?        | yes      | optional                 |
| Phone      | string?        | yes      | max length 20            |
| Address    | string?        | yes      | max length 250           |
| Notes      | string?        | yes      | max length 1000          |
| CreatedAt  | DateTimeOffset | no       | timestamp of creation    |
| UpdatedAt  | DateTimeOffset | no       | timestamp of last update |

### Relationships

| Cardinality                               | Type | Optional/Required | On Delete Behavior | Notes                                   |
| ----------------------------------------- | ---- | ----------------- | ------------------ | --------------------------------------- |
| 1 **Profile** → many **Doctors**          | 1:N  | required          | Cascade            | Each doctor belongs to a profile        |
| 1 **Doctor** → many **Appointments**      | 1:N  | optional          | SetNull            | Appointments linked to doctor           |
| 1 **Doctor** → many **ProfileMedication** | 1:N  | optional          | SetNull            | Medication records prescribed by doctor |

### Behavior / Business Rules (Entity Level)

- Cannot exist without a parent profile.  
- Name is required; Specialty, Phone, Address, Notes are optional.  
- Doctors are profile-specific and cannot be shared across profiles.  
- Deleting a doctor sets associated appointments or medications’ doctor reference to null.

### Edge Cases

- No doctors exist → user can create one from the doctor screen or while scheduling an appointment.  
- Deleting a doctor with linked appointments or medications → reference is nullified to maintain history.

### Explicitly NOT included (for MVP)

- Multiple phone numbers or emails per doctor.  
- Categories or tags for doctors.  
- Integration with external provider directories.

## Appointment

### Purpose

Represents a scheduled visit or event for a profile, optionally associated with a doctor.

### Entity Type

- Transactional / Event

### Fields

| Field Name | Type           | Nullable | Notes                                               |
| ---------- | -------------- | -------- | --------------------------------------------------- |
| Id         | int            | no       | primary key                                         |
| ProfileId  | int            | no       | foreign key to Profile                              |
| DoctorId   | int?           | yes      | foreign key to Doctor (optional)                    |
| DateTime   | DateTimeOffset | no       | date and time of the appointment                    |
| Reason     | string?        | yes      | optional reason or purpose of visit, max length 250 |
| Notes      | string?        | yes      | optional notes, max length 1000                     |
| CreatedAt  | DateTimeOffset | no       | timestamp of creation                               |
| UpdatedAt  | DateTimeOffset | no       | timestamp of last update                            |

### Relationships

| Cardinality                            | Type | Optional/Required | On Delete Behavior | Notes                                    |
| -------------------------------------- | ---- | ----------------- | ------------------ | ---------------------------------------- |
| 1 **Profile** → many **Appointments**  | 1:N  | required          | Cascade            | Appointments belong to a profile         |
| 1 **Doctor** → many **Appointments**   | 1:N  | optional          | SetNull            | Appointment may or may not have a doctor |
| 1 **Appointment** → many **Notes**     | 1:N  | optional          | SetNull            | Appointment-linked notes                 |
| 1 **Appointment** → many **Documents** | 1:N  | optional          | SetNull            | Appointment-linked documents             |

### Behavior / Business Rules (Entity Level)

- Cannot exist without a parent profile.  
- Doctor association is optional; some appointments may not have a doctor.  
- Appointments must have a valid DateTime.  
- Notes and Documents linked to deleted appointments should have their AppointmentId set to null to preserve history.  

### Edge Cases

- No appointments exist → system allows creating one from the profile dashboard.  
- Appointment linked to a doctor who is later deleted → doctor reference is nullified.  
- Appointment linked to notes or documents → deletion sets foreign keys to null, does not delete related records.  

### Explicitly NOT included (for MVP)

- Recurring appointments or series.  
- Automatic reminders or notifications.  
- Complex scheduling rules or availability checking.

## Condition

### Purpose

Represents a medical condition, diagnosis, or health issue that can be associated with a profile.

### Entity Type

- Lookup

### Fields

| Field Name | Type           | Nullable | Notes                    |
| ---------- | -------------- | -------- | ------------------------ |
| Id         | int            | no       | primary key              |
| Name       | string         | no       | unique, max length 150   |
| CreatedAt  | DateTimeOffset | no       | timestamp of creation    |
| UpdatedAt  | DateTimeOffset | no       | timestamp of last update |

### Relationships

| Cardinality                                  | Type | Optional/Required | On Delete Behavior | Notes                                |
| -------------------------------------------- | ---- | ----------------- | ------------------ | ------------------------------------ |
| 1 **Condition** → many **ProfileCondition**  | 1:N  | required          | Cascade            | Links condition to profiles          |
| 1 **Condition** → many **ProfileMedication** | 1:N  | optional          | SetNull            | Medication may reference a condition |

### Behavior / Business Rules (Entity Level)

- Condition names must be unique.  
- Cannot be deleted if referenced by ProfileCondition without handling dependencies (Cascade allows automatic removal).  
- Serves as a standard reference for linking health records and medications.  

### Edge Cases

- Attempting to delete a condition that is actively used by a profile → dependent records cascade or must be handled.  
- Condition name collisions → system enforces uniqueness.  

### Explicitly NOT included (for MVP)

- Hierarchies or categories of conditions.  
- Detailed metadata about conditions (e.g., ICD codes, severity, onset type).

## ProfileCondition

### Purpose

Links a profile to a medical condition, capturing diagnosis details and relevant notes.

### Entity Type

- Associative / Join

### Fields

| Field Name    | Type           | Nullable | Notes                      |
| ------------- | -------------- | -------- | -------------------------- |
| Id            | int            | no       | primary key                |
| ProfileId     | int            | no       | foreign key → Profile.Id   |
| ConditionId   | int            | no       | foreign key → Condition.Id |
| DiagnosedDate | DateTime?      | yes      | optional date of diagnosis |
| Notes         | string?        | yes      | max length 1000            |
| CreatedAt     | DateTimeOffset | no       | timestamp of creation      |
| UpdatedAt     | DateTimeOffset | no       | timestamp of last update   |

### Relationships

| Cardinality                                 | Type | Optional/Required | On Delete Behavior | Notes                |
| ------------------------------------------- | ---- | ----------------- | ------------------ | -------------------- |
| 1 **Profile** → many **ProfileCondition**   | 1:N  | required          | Cascade            | Profile must exist   |
| 1 **Condition** → many **ProfileCondition** | 1:N  | required          | Cascade            | Condition must exist |

### Behavior / Business Rules (Entity Level)

- Each ProfileCondition must link a valid profile and a valid condition.  
- Allows multiple conditions per profile and multiple profiles per condition.  
- Notes and diagnosed date are optional but can provide additional context.  

### Edge Cases

- Deleting a profile automatically deletes associated ProfileCondition entries.  
- Deleting a condition automatically deletes associated ProfileCondition entries.  
- Duplicate entries for the same profile and condition should be prevented by application logic.  

### Explicitly NOT included (for MVP)

- Tracking of condition severity, progression, or treatment history.  
- Complex condition hierarchies or relationships between conditions.

## Medication

### Purpose

Represents a master list of medications, including brand and generic names, used for tracking prescriptions and usage.

### Entity Type

- Lookup

### Fields

| Field Name  | Type           | Nullable | Notes                                         |
| ----------- | -------------- | -------- | --------------------------------------------- |
| Id          | int            | no       | primary key                                   |
| Name        | string         | no       | unique, max length 150, used for autocomplete |
| GenericName | string?        | yes      | optional generic name, max length 150         |
| BrandName   | string?        | yes      | optional brand name, max length 150           |
| Notes       | string?        | yes      | optional notes, max length 1000               |
| CreatedAt   | DateTimeOffset | no       | timestamp of creation                         |
| UpdatedAt   | DateTimeOffset | no       | timestamp of last update                      |

### Relationships

| Cardinality                                   | Type | Optional/Required | On Delete Behavior | Notes                                       |
| --------------------------------------------- | ---- | ----------------- | ------------------ | ------------------------------------------- |
| 1 **Medication** → many **ProfileMedication** | 1:N  | required          | Cascade            | Tracks usage of this medication by profiles |

### Behavior / Business Rules (Entity Level)

- Medication names must be unique in the system.  
- Optional fields (generic, brand, notes) provide additional context but are not required.  
- Does not track which profile is taking it—this is handled by **ProfileMedication**.  

### Edge Cases

- Deleting a medication cascades to all ProfileMedication records referencing it.  
- Medication without any usage (no ProfileMedication) is allowed.  

### Explicitly NOT included (for MVP)

- Dosage history or detailed schedules (handled via ProfileMedication).  
- Interactions, contraindications, or off-label usage tracking.

## ProfileMedication

### Purpose

Represents a medication assigned to a specific profile, including usage instructions, associated conditions, and prescribing doctor.

### Entity Type

- Transactional / Associative

### Fields

| Field Name   | Type            | Nullable | Notes                                                                   |
| ------------ | --------------- | -------- | ----------------------------------------------------------------------- |
| Id           | int             | no       | primary key                                                             |
| ProfileId    | int             | no       | FK → Profile.Id                                                         |
| MedicationId | int             | no       | FK → Medication.Id                                                      |
| DoctorId     | int?            | yes      | FK → Doctor.Id, optional prescribing doctor                             |
| ConditionId  | int?            | yes      | FK → ProfileCondition.Id or Condition.Id, optional associated condition |
| StartDate    | DateTimeOffset  | no       | start of medication usage                                               |
| EndDate      | DateTimeOffset? | yes      | null if ongoing; must be ≥ StartDate if present                         |
| Instructions | string?         | yes      | e.g., "2x per day", max length 500                                      |
| Notes        | string?         | yes      | max length 1000                                                         |
| CreatedAt    | DateTimeOffset  | no       | timestamp of creation                                                   |
| UpdatedAt    | DateTimeOffset  | no       | timestamp of last update                                                |

### Relationships

| Cardinality                                   | Type | Optional/Required | On Delete Behavior | Notes                                           |
| --------------------------------------------- | ---- | ----------------- | ------------------ | ----------------------------------------------- |
| 1 **Profile** → many **ProfileMedication**    | 1:N  | required          | Cascade            | A profile can have multiple medications         |
| 1 **Medication** → many **ProfileMedication** | 1:N  | required          | Cascade            | Tracks which profiles are taking the medication |
| 1 **Doctor** → many **ProfileMedication**     | 1:N  | optional          | SetNull            | Prescribing doctor                              |
| 1 **Condition** → many **ProfileMedication**  | 1:N  | optional          | SetNull            | Associated condition if known                   |

### Behavior / Business Rules (Entity Level)

- ProfileMedication cannot exist without a parent profile and a valid medication.  
- StartDate is required; EndDate must be null or ≥ StartDate.  
- Instructions are optional but recommended for clarity.  
- A single medication can be linked to multiple profiles, and a profile can have multiple medications.  
- Association with a condition is optional; not all medications will have a known indication.  

### Edge Cases

- Deleting a profile cascades to all associated ProfileMedication records.  
- Deleting a medication cascades to all ProfileMedication records.  
- Deleting a doctor or condition sets the respective FK to null; ProfileMedication remains.  
- Medications with overlapping dates for the same profile are allowed.  

### Explicitly NOT included (for MVP)

- Detailed dosage schedules (per-time-of-day logging)  
- Medication interactions or contraindications  
- Automated reminders or adherence tracking

## SymptomLog

### Purpose

Tracks the occurrence and severity of symptoms for a profile over time, optionally linked to an appointment.

### Entity Type

- Log / Transactional

### Fields

| Field Name    | Type           | Nullable | Notes                              |
| ------------- | -------------- | -------- | ---------------------------------- |
| Id            | int            | no       | primary key                        |
| ProfileId     | int            | no       | FK → Profile.Id                    |
| SymptomId     | int            | no       | FK → Symptom.Id                    |
| DateTime      | DateTimeOffset | no       | timestamp of symptom occurrence    |
| Severity      | int            | no       | scale 1–10                         |
| Notes         | string?        | yes      | optional details, max length 1000  |
| AppointmentId | int?           | yes      | FK → Appointment.Id, optional link |
| CreatedAt     | DateTimeOffset | no       | timestamp of creation              |
| UpdatedAt     | DateTimeOffset | no       | timestamp of last update           |

### Relationships

| Cardinality                             | Type | Optional/Required | On Delete Behavior | Notes                                              |
| --------------------------------------- | ---- | ----------------- | ------------------ | -------------------------------------------------- |
| 1 **Profile** → many **SymptomLog**     | 1:N  | required          | Cascade            | A profile can log multiple symptoms                |
| 1 **Symptom** → many **SymptomLog**     | 1:N  | required          | Cascade            | Defines what symptom is logged                     |
| 1 **Appointment** → many **SymptomLog** | 1:N  | optional          | SetNull            | Links symptom logs to appointments when applicable |

### Behavior / Business Rules (Entity Level)

- Cannot exist without a valid profile and symptom.  
- Severity is required and must be within the defined range (1–10).  
- Logs can be freestanding or tied to an appointment.  
- Multiple logs for the same symptom on the same day are allowed.  

### Edge Cases

- Deleting a profile cascades to all associated SymptomLogs.  
- Deleting a symptom cascades to all associated SymptomLogs.  
- Deleting an appointment sets AppointmentId to null; logs remain.  
- Users may log symptoms retrospectively (DateTime in the past).  

### Explicitly NOT included (for MVP)

- Symptom aggregation, trends, or visualization  
- Automatic severity scoring or recommendations  
- Integration with external health devices or sensors

## Note

### Purpose

Represents free-form notes for a profile, which can be standalone or linked to a specific appointment.

### Entity Type

- Transactional / Log

### Fields

| Field Name    | Type           | Nullable | Notes                                     |
| ------------- | -------------- | -------- | ----------------------------------------- |
| Id            | int            | no       | primary key                               |
| ProfileId     | int            | no       | FK → Profile.Id                           |
| AppointmentId | int?           | yes      | FK → Appointment.Id, optional link        |
| Content       | string         | no       | text content of the note, max length 2000 |
| CreatedAt     | DateTimeOffset | no       | timestamp of creation                     |
| UpdatedAt     | DateTimeOffset | no       | timestamp of last update                  |

### Relationships

| Cardinality                       | Type | Optional/Required | On Delete Behavior | Notes                               |
| --------------------------------- | ---- | ----------------- | ------------------ | ----------------------------------- |
| 1 **Profile** → many **Note**     | 1:N  | required          | Cascade            | All notes belong to a profile       |
| 1 **Appointment** → many **Note** | 1:N  | optional          | SetNull            | Notes can be tied to an appointment |

### Behavior / Business Rules (Entity Level)

- Cannot exist without a profile.  
- Appointment link is optional.  
- Notes can be created, updated, and deleted independently.  
- Notes do not enforce uniqueness; multiple notes with the same content are allowed.  

### Edge Cases

- Deleting a profile cascades to all associated notes.  
- Deleting an appointment sets AppointmentId to null; notes remain.  
- Notes can be added for past or future appointments.  

### Explicitly NOT included (for MVP)

- Rich text formatting or attachments  
- Tagging or categorization  
- Search indexing beyond simple text search

## Document

### Purpose

Represents uploaded or scanned files related to a profile or a specific appointment. Can include lab results, imaging, discharge summaries, or general medical documents.

### Entity Type

- Transactional / Log

### Fields

| Field Name    | Type           | Nullable | Notes                                              |
| ------------- | -------------- | -------- | -------------------------------------------------- |
| Id            | int            | no       | primary key                                        |
| ProfileId     | int            | no       | FK → Profile.Id                                    |
| AppointmentId | int?           | yes      | FK → Appointment.Id, optional link                 |
| FilePath      | string         | no       | path to file on disk, max length 500               |
| FileName      | string         | no       | original filename, max length 255                  |
| ContentType   | string         | no       | MIME type, e.g., "application/pdf", max length 100 |
| MediaType     | string?        | yes      | e.g., "Document", "Image", "Scan"                  |
| FileSize      | long?          | yes      | size in bytes                                      |
| UploadedAt    | DateTimeOffset | no       | timestamp when file was uploaded                   |
| Notes         | string?        | yes      | optional description or comments, max length 1000  |
| CreatedAt     | DateTimeOffset | no       | creation timestamp                                 |
| UpdatedAt     | DateTimeOffset | no       | last updated timestamp                             |

### Relationships

| Cardinality                           | Type | Optional/Required | On Delete Behavior | Notes                                              |
| ------------------------------------- | ---- | ----------------- | ------------------ | -------------------------------------------------- |
| 1 **Profile** → many **Document**     | 1:N  | required          | Cascade            | All documents belong to a profile                  |
| 1 **Appointment** → many **Document** | 1:N  | optional          | SetNull            | Documents can optionally be tied to an appointment |

### Behavior / Business Rules (Entity Level)

- Cannot exist without a profile.  
- Appointment link is optional.  
- Documents must have a valid file path and content type.  
- Documents can be uploaded, updated (metadata), or deleted.  

### Edge Cases

- Deleting a profile cascades to all associated documents.  
- Deleting an appointment sets AppointmentId to null; documents remain attached to profile.  
- File storage failures or missing files should be handled gracefully.  

### Explicitly NOT included (for MVP)

- Versioning of documents  
- Online preview or rendering inside the app  
- Integration with cloud storage or external systems  

EOL
