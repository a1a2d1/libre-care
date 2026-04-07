## Profile System — MVP Business Rules

### Core Invariant

* The system must always have **exactly one active Profile** when at least one profile exists.

---

### Profile Creation

* A user can create a Profile with:

  * Name (required)
  * BirthDate (optional)

* When creating a Profile:

  * If **no profiles exist**, the new Profile is automatically set to `IsActive = true`
  * If profiles already exist:

    * The new Profile is set to `IsActive = true`
    * All other profiles are set to `IsActive = false`

---

### Active Profile Rules

* At any time, **only one Profile can have `IsActive = true`**
* Setting a Profile as active:

  * Sets `IsActive = true` for the selected profile
  * Sets `IsActive = false` for all other profiles

---

### Profile Deletion

* When deleting a Profile:

  * If the deleted Profile is **not active**:

    * No additional action is required
  * If the deleted Profile **is active**:

    * The system selects the **first available remaining Profile**
    * Sets it as `IsActive = true`
  * If no profiles remain:

    * No active profile exists

---

### Profile Editing

* A user can update:

  * Name
  * BirthDate
* Editing a Profile does **not** affect `IsActive`

---

### Application Entry (Redirect Logic)

On application load (`/` route):

1. Retrieve all Profiles

2. If **no profiles exist**:

   * Redirect to:

     ```
     /Profiles/Create
     ```

3. If profiles exist:

   * Retrieve the active Profile

4. If an active Profile exists:

   * Redirect to:

     ```
     /Profiles/Dashboard/{id}
     ```

5. If no active Profile exists (fallback safety case):

   * Select the first Profile
   * Set it as active
   * Redirect to its Dashboard

---

### Dashboard Behavior

* Dashboard is always scoped to a specific Profile:

  ```
  /Profiles/Dashboard/{id}
  ```

* The Dashboard:

  * Loads the Profile by `id`
  * Displays Profile-specific data (future slices)

---

### Non-MVP (Deferred)

The following are intentionally NOT implemented in MVP:

* Multiple active profiles
* No-active-profile state
* Profile selection screen (Netflix-style)
* Profile-level permissions
* Soft delete / archival
* Profile export on delete
