# LibreCare MVP Functional Requirements

## Profile

### Purpose

A Profile represents a person whose health information is managed in the
system (self, family member, or dependent).

### Core Behaviors

#### Create Profile

- User can create a new profile
- Profile requres a name
- Profile may include optional basic details:
  - Date of birth
  - Sex/gender
  - Notes

#### View Profiles

- User can view a list of all profiles
- Each profile displays at minimum:
  - Name
  - Age/DOB (optional/if available)

#### Select Active Profile

- User can select a profile as the **active profile**
- System clearly indicates which profile is currently active
- All data viewed and created is scoped to the active profile

#### Edit Profile

- User can update all profile fields.
- Changes are reflected immediately in the system

#### Delete Profile

- User can delete a profile
- System prevents accidental deletion (confirmation required)
- Deleting a profile removes or archives all associated data
  - Cascade vs. soft delete will be specified in db schema

#### Relationships

- Profile contains:
  - Doctors/Contacts
  - Appointments
  - Conditions
  - Medications
  - Allergies
  - Symptom logs
  - Notes
  - Documents

All records in the system must belong to exactly one Profile.

#### Key Behaviors / Rules

- System must always have **one active profile selected** when data is being viewed or created
- User cannot create records (appointments, meds, etc.) without an active profile
- Switching profiles updates all visible data to that profile’s data
- Profiles are independent (no shared records between profiles)

#### Edge Cases

- If no profiles exist:
  - System prompts user to create one before doing anything else
- If only one profile exists:
  - It is automatically selected as active
- If active profile is deleted:
  - System must require selecting or creating another profile before continuing

#### Explicitly NOT included (for MVP)

- Profile-level permissions
- Profile-level passwords/PINs
- Profile sharing across devices/users
- Avatars/photos (optional later)

#### Done Definition

You are “done” with Profile when:

- You can create 2+ profiles
- You can switch between them
- Creating data (even a placeholder like a note) respects the active profile

## Doctor

### Purpose

A Doctor represents a medical provider associated with a Profile and used fo
managing care and appointments.

### Core Behaviors

#### Add Doctor

- User can add a doctor to a profile
- Doctor requires a name
- Doctor may include:
  - Specialty (e.g., cardiology, primary care)
  - Phone number
  - Address
  - Notes

#### View Doctors

- User can view a list of all doctors for the active profile
- Each doctor displays at minimum:
  - Name
  - Specialty/type (optional)

#### View Doctor Details

- User can view full details of a doctor
- Includes all entered fields and related information (e.g., appointments)

#### Edit Doctor

- User can update all doctor fields

#### Delete Doctor

- User can delete a doctor
- System requires confirmation before deletion

#### Relationships (Functional Perspective)

- A Doctor:
  - Belongs to one Profile
  - Can be associated with multiple Appointments
- A Profile:
  - Can have multiple Doctors

#### Key Behaviors/Rules

- A doctor is always created within the context of the **active profile**
- Doctors are **profile-specific** (no sharing across profiles)
- When creating or editing an appointment:
  - User can select an existing doctor
  - User can create a new doctor inline (optional)

#### Appointment Integration

- System allows linking an appointment to a doctor
- A doctor can exist without appointments
- An appointment may:
  - Require a doctor (recommended)
  - Or allow “no doctor” (e.g., lab visit) → your call, but default should be with doctor

#### Flexibility

- System does not enforce strict doctor types
- Users may enter:
  - Pharmacy
  - Therapist
  - Clinic
  - Any provider needed

#### Edge Cases

- If no doctors exist:
  - System allows creating one from:
    - Doctor screen
    - Appointment flow
- If doctor is delted:
  - System must handle associated appointments:
    - Prevent deletion if linked
    - OR remove the association from those appointments

#### Explicitly NOT included (for MVP)

- Contact categories (no “Contact system”)
- Multiple phone numbers/emails
- Validation of address/phone formats
- Integration with external provider databases
- Favorites, tags, or advanced filtering

#### Done Definition

You are “done” with Doctor when:

- You can create doctors per profile
- You can select a doctor when creating an appointment
- You can view all appointments tied to a doctor (even a simple list)

## Appointment

### Purpose

An Appointment represents a scheduled medical visit or event for a Profile, used
to manage care and prepare for interactions with providers.

### Core Behaviors

#### Create Appointment

- User can create an appointment for the active profile
- Appointment requires:
  - Date
- Appointment may include:
  - Time
  - Doctor
  - Location
  - Reason for visit
  - Notes

#### View Appointments

- User can view a list of all appointments for the active profile
- Appointments are displayed in chronological order (upcoming first)
- Each appointment displays at minimum:
  - Date
  - Time (if available)
  - Doctor (if assigned)

#### View Appointment Details

- User can view full details of an appointment
- Includes all entered fields and related information

#### Edit Appointment

- User can update all appointment fields

#### Delete Appointment

- User can delete an appointment
- System requires confirmation before deletion

#### Relationships (Functional Perspective)

- An Appointment:
  - Belongs to one Profile
  - May be associated with one Doctor
- A Profile:
  - Can have multiple Appointments
- A Doctor:
  - Can be associated with multiple Appointments

#### Key Behaviors / Rules

- Appointment is always created within the context of the **active profile**
- Appointment must have a valid date
- Time is optional (to support all-day or unknown-time events)
- When assigning a doctor:
  - User selects from existing doctors
  - User may create a new doctor inline
- Appointments are profile-specific (no sharing across profiles)

#### Status / Time Awareness (MVP-lite)

- System distinguishes:
  - Upcoming appointments (future date/time)
  - Past appointments (past date/time)
- No advanced status system (e.g., canceled, rescheduled) in MVP

#### Practical Behaviors

- User can create appointments without assigning a doctor (allowed for flexibility)
- User can use appointments for:
  - Doctor visits
  - Lab visits
  - Therapy sessions
  - Any care-related event

#### Edge Cases

- If no doctors exist:
  - User can still create an appointment without a doctor
  - Or create a doctor inline
- If associated doctor is deleted:
  - Appointment remains
  - Doctor field is cleared

#### Explicitly NOT included (for MVP)

- Appointment reminders/notifications
- Calendar integrations (Google, Outlook, etc.)
- Recurring appointments
- Appointment status tracking (canceled, no-show, etc.)
- Time zone handling beyond basic local system time

#### Done Definition

You are “done” with Appointment when:

- You can create appointments tied to a profile
- You can optionally assign a doctor
- You can clearly distinguish upcoming vs past appointments
- You can view and edit appointment details without errors

## Medication

### Purpose

A Medication represents a drug or treatment that a Profile is taking or has taken, including dosage and usage details.

### Core Behaviors

#### Add Medication

- User can add a medication to the active profile
- User can search and select a medication from a predefined list
- System provides autosuggestions as user types
- System populates known details when a medication is selected
- User can create a custom medication if not found

- Medication requires:
  - Name (selected or entered)

- Medication may include:
  - Dosage (e.g., 500mg)
  - Frequency (e.g., twice daily)
  - Route (e.g., oral, injection) (optional)
  - Start date
  - End date
  - Instructions/notes

---

#### View Medications

- User can view a list of all medications for the active profile
- Medications are grouped or filterable by:
  - Active (currently taking)
  - Past (no longer taking)

- Each medication displays at minimum:
  - Name
  - Dosage (if available)
  - Frequency (if available)
  - Status (active/past)

---

#### View Medication Details

- User can view full details of a medication
- Includes all entered fields and any associated notes

---

#### Edit Medication

- User can update all medication fields
- User can modify dosage, frequency, and instructions over time

---

#### Stop Medication

- User can mark a medication as no longer being taken
- System sets or updates the end date

---

#### Delete Medication

- User can delete a medication
- System requires confirmation before deletion

---

### Relationships (Functional Perspective)

- A Medication:
  - Belongs to one Profile

- A Profile:
  - Can have multiple Medications

---

### Key Behaviors / Rules

- Medication is always created within the context of the **active profile**
- System supports both:
  - Selecting from predefined medication list
  - Creating custom entries
- Predefined medication data is read-only (user cannot modify base data)
- User-specific details (dosage, frequency, etc.) are editable
- A medication is considered **active** if:
  - No end date is set
- A medication is considered **past** if:
  - End date is set

---

### Data Entry Behavior (Important)

- Autosuggest triggers as user types medication name
- Selecting a suggestion:
  - Fills in known/default fields (if available)
- User can override or add custom instructions regardless of selection

---

### Practical Behaviors

- User can track:
  - Prescription medications
  - Over-the-counter medications
  - Supplements
- System does not enforce strict validation on dosage/frequency format

---

### Edge Cases

- If medication is not found in predefined list:
  - User can create a custom medication entry
- If user clears or removes predefined selection:
  - Custom name remains
- If medication has no dosage/frequency:
  - It is still valid

---

### Explicitly NOT included (for MVP)

- Medication reminders/alerts
- Drug interaction warnings
- Dosage validation or safety checks
- Integration with external pharmacy systems
- Refill tracking
- Multiple concurrent dosage schedules (keep simple for now)

---

### Done Definition

You are “done” with Medication when:

- You can add medications using autosuggest or custom entry
- You can distinguish active vs past medications
- You can update dosage and instructions
- You can stop (end) a medication cleanly

## Condition

### Purpose

A Condition represents a medical diagnosis or health condition associated with a Profile, used to track ongoing or past health issues.

### Core Behaviors

#### Add Condition

- User can add a condition to the active profile
- User can search and select a condition from a predefined list
- System provides autosuggestions as user types
- System populates known details when a condition is selected
- User can create a custom condition if not found

- Condition requires:
  - Name (selected or entered)

- Condition may include:
  - Status (e.g., active, resolved) (optional)
  - Diagnosis date (optional)
  - Notes

---

#### View Conditions

- User can view a list of all conditions for the active profile
- Conditions are grouped or filterable by:
  - Active
  - Past (resolved)

- Each condition displays at minimum:
  - Name
  - Status (if available)

---

#### View Condition Details

- User can view full details of a condition
- Includes all entered fields and notes

---

#### Edit Condition

- User can update all condition fields

---

#### Resolve Condition

- User can mark a condition as resolved
- System updates status accordingly

---

#### Delete Condition

- User can delete a condition
- System requires confirmation before deletion

---

### Relationships (Functional Perspective)

- A Condition:
  - Belongs to one Profile

- A Profile:
  - Can have multiple Conditions

---

### Key Behaviors / Rules

- Condition is always created within the context of the **active profile**
- System supports both:
  - Selecting from predefined condition list
  - Creating custom entries
- Predefined condition data is read-only
- User-specific details (status, notes, dates) are editable
- A condition is considered **active** if:
  - Not marked as resolved
- A condition is considered **past** if:
  - Marked as resolved

---

### Data Entry Behavior

- Autosuggest triggers as user types condition name
- Selecting a suggestion:
  - Fills in known/default fields (if available)
- User can override or add notes regardless of selection

---

### Practical Behaviors

- User can track:
  - Chronic conditions (e.g., diabetes)
  - Acute conditions (e.g., flu)
  - General health concerns
- System does not enforce strict medical validation

---

### Edge Cases

- If condition is not found in predefined list:
  - User can create a custom condition
- If no status is selected:
  - Condition defaults to active

---

### Explicitly NOT included (for MVP)

- Severity tracking
- Condition hierarchies or grouping
- Clinical coding systems (ICD, SNOMED, etc.)
- Integration with external medical systems

---

### Done Definition

You are “done” with Condition when:

- You can add conditions using autosuggest or custom entry
- You can mark conditions as active or resolved
- You can view and edit condition details

## Allergy

### Purpose

An Allergy represents a known allergic reaction or sensitivity for a Profile, used to track substances that should be avoided.

### Core Behaviors

#### Add Allergy

- User can add an allergy to the active profile
- User can search and select an allergy from a predefined list (optional)
- System provides autosuggestions as user types (if list is available)
- User can create a custom allergy entry

- Allergy requires:
  - Substance name (e.g., penicillin, peanuts)

- Allergy may include:
  - Reaction (e.g., rash, anaphylaxis)
  - Severity (optional, simple value)
  - Notes

---

#### View Allergies

- User can view a list of all allergies for the active profile
- Each allergy displays at minimum:
  - Substance name
  - Reaction (if available)
  - Severity (if available)

---

#### View Allergy Details

- User can view full details of an allergy
- Includes all entered fields and notes

---

#### Edit Allergy

- User can update all allergy fields

---

#### Delete Allergy

- User can delete an allergy
- System requires confirmation before deletion

---

### Relationships (Functional Perspective)

- An Allergy:
  - Belongs to one Profile

- A Profile:
  - Can have multiple Allergies

---

### Key Behaviors / Rules

- Allergy is always created within the context of the **active profile**
- System supports:
  - Free-text entry (required)
  - Optional autosuggest from predefined list (if implemented)
- User-entered data is fully editable
- Allergy records are always considered **active** (no resolved state in MVP)

---

### Data Entry Behavior

- Autosuggest may trigger as user types (optional for MVP)
- User can enter any custom substance name
- No strict validation on reaction or severity fields

---

### Practical Behaviors

- User can track:
  - Drug allergies
  - Food allergies
  - Environmental allergies (e.g., pollen)
- System prioritizes flexibility over strict structure

---

### Edge Cases

- Duplicate allergies (same substance) are allowed but not prevented
- If no reaction or severity is provided:
  - Allergy is still valid

---

### Explicitly NOT included (for MVP)

- Allergy interaction warnings (e.g., with medications)
- Severity standardization or enforcement
- Clinical coding systems
- Integration with external medical systems

---

### Done Definition

You are “done” with Allergy when:

- You can add allergies with minimal required input
- You can view and edit allergy details
- You can maintain a clear list of known allergies per profile

## Symptom Log

### Purpose

A Symptom Log represents a recorded observation of symptoms for a Profile at a specific point in time, used to track patterns and support medical discussions.

### Core Behaviors

#### Add Symptom Entry

- User can add a symptom log entry for the active profile
- Entry requires:
  - Date (defaults to current date)
- Entry may include:
  - Time (optional)
  - Symptom name/description
  - Severity (optional, simple scale or free text)
  - Notes

---

#### View Symptom Logs

- User can view a list of symptom entries for the active profile
- Entries are displayed in reverse chronological order (most recent first)

- Each entry displays at minimum:
  - Date
  - Symptom name/description (if available)
  - Severity (if available)

---

#### View Symptom Entry Details

- User can view full details of a symptom entry
- Includes all entered fields

---

#### Edit Symptom Entry

- User can update all fields of a symptom entry

---

#### Delete Symptom Entry

- User can delete a symptom entry
- System requires confirmation before deletion

---

### Relationships (Functional Perspective)

- A Symptom Log Entry:
  - Belongs to one Profile

- A Profile:
  - Can have multiple Symptom Log Entries

---

### Key Behaviors / Rules

- Symptom entries are always created within the context of the **active profile**
- Each entry represents a **point-in-time record** (no aggregation in MVP)
- Date is required; time is optional
- System does not enforce structured symptom taxonomy (free text allowed)

---

### Data Entry Behavior

- System may support simple predefined symptom suggestions (optional)
- User can always enter free-text symptom descriptions
- Severity can be:
  - Numeric (e.g., 1–10)
  - Or free text (implementation choice, keep simple)

---

### Practical Behaviors

- User can log:
  - Single symptoms (e.g., headache)
  - Multiple symptoms in notes (simple approach for MVP)
- System supports quick entry (low friction is important)

---

### Edge Cases

- If no symptom description is entered:
  - Entry is still valid if notes are provided
- Multiple entries can exist for the same day/time
- No deduplication or merging of entries

---

### Explicitly NOT included (for MVP)

- Charts, graphs, or trend analysis
- Symptom correlation with medications or conditions
- Reminders to log symptoms
- Structured multi-symptom tracking per entry (keep simple)
- Export/analytics features

---

### Done Definition

You are “done” with Symptom Log when:

- You can quickly add symptom entries with minimal friction
- Entries are clearly ordered by date
- You can view and edit past entries easily

## Note

### Purpose

A Note represents a general piece of information, observation, or record for a Profile, optionally linked to an Appointment.

### Core Behaviors

#### Create Note

- User can create a note for the active profile
- Note requires:
  - Content (text)

- Note may include:
  - Title (optional)
  - Date (defaults to current date)
  - Associated appointment (optional)

---

#### View Notes

- User can view a list of all notes for the active profile
- Notes are displayed in reverse chronological order (most recent first)

- Each note displays at minimum:
  - Title (if available) or truncated content
  - Date

---

#### View Note Details

- User can view full details of a note
- Includes all entered fields

---

#### Edit Note

- User can update all note fields

---

#### Delete Note

- User can delete a note
- System requires confirmation before deletion

---

### Relationships (Functional Perspective)

- A Note:
  - Belongs to one Profile
  - May be associated with one Appointment

- A Profile:
  - Can have multiple Notes

- An Appointment:
  - Can have multiple associated Notes

---

### Key Behaviors / Rules

- Note is always created within the context of the **active profile**
- Notes are flexible and unstructured (free-text focused)
- Linking a note to an appointment is optional
- Notes remain valid even if not linked to any appointment

---

### Data Entry Behavior

- Date defaults to current date but can be adjusted
- Title is optional; system can fall back to content preview in lists
- No strict formatting or structure is enforced on content

---

### Practical Behaviors

- User can use notes for:
  - General health observations
  - Questions for future doctor visits
  - Visit summaries (if more detail is needed beyond appointment notes)
- Notes can act as a “catch-all” for information not covered by other entities

---

### Edge Cases

- If linked appointment is deleted:
  - Note remains
  - Association to appointment is removed
- If no title is provided:
  - System displays truncated content in lists
- Empty content is not allowed

---

### Explicitly NOT included (for MVP)

- Rich text formatting (bold, lists, etc.)
- Tags, categories, or labels
- Search within note content (can add later)
- Version history or audit tracking
- Attachments (handled separately via Documents)

---

### Done Definition

You are “done” with Note when:

- You can create and edit notes quickly
- You can optionally link notes to appointments
- Notes are easy to browse and review over time

## Document

### Purpose

A Document represents an uploaded or stored file associated with a Profile, such as medical records, lab results, imaging, or other health-related files.

### Core Behaviors

#### Add Document

- User can add a document to the active profile
- User uploads a file from the local system

- Document requires:
  - File

- Document may include:
  - Title (optional)
  - Description/notes (optional)
  - Date (defaults to current date)
  - Associated appointment (optional)

---

#### View Documents

- User can view a list of all documents for the active profile

- Each document displays at minimum:
  - Title (or file name if no title provided)
  - Date

---

#### View/Open Document

- User can open a document
- System uses the operating system’s default application to view the file

---

#### Edit Document

- User can update document metadata:
  - Title
  - Description/notes
  - Date
  - Associated appointment

- User cannot edit the file content within the app

---

#### Delete Document

- User can delete a document
- System requires confirmation before deletion
- Deleting a document removes the file from storage

---

### Relationships (Functional Perspective)

- A Document:
  - Belongs to one Profile
  - May be associated with one Appointment

- A Profile:
  - Can have multiple Documents

- An Appointment:
  - Can have multiple associated Documents

---

### Key Behaviors / Rules

- Document is always created within the context of the **active profile**
- Files are stored locally on the user’s system
- Document metadata is managed within the app
- Linking a document to an appointment is optional
- Documents remain valid even if not linked to any appointment

---

### File Handling Behavior

- System stores a reference to the file and/or manages file storage internally (implementation detail)
- File name is preserved unless user overrides with a title
- System does not restrict file types (flexible for MVP)

---

### Practical Behaviors

- User can store:
  - Visit summaries / discharge papers
  - Lab results
  - Imaging files (e.g., X-rays, scans)
  - Insurance documents
  - Any relevant health-related files

---

### Edge Cases

- If linked appointment is deleted:
  - Document remains
  - Association to appointment is removed
- If file is missing or moved outside the app:
  - System should handle error gracefully when opening
- If no title is provided:
  - File name is used for display

---

### Explicitly NOT included (for MVP)

- File previews within the app
- OCR or text extraction
- File type validation or restrictions
- Cloud storage or sync
- Versioning of documents
- Drag-and-drop organization or folders

---

### Done Definition

You are “done” with Document when:

- You can upload and store files per profile
- You can open files using the system default application
- You can optionally link documents to appointments
- Documents are easy to list and access
