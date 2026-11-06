# ECLIPSE: THE LAST CIVILIZATION
## UNUSED CODE REPORT

### 1. Overview
This audit cataloged all unused variables, duplicate declarations, obsolete functions, and dead CSS rules across the frontend and backend of *ECLIPSE: THE LAST CIVILIZATION*.

---

### 2. Frontend Dead Code & Cleaned Items
1. **Duplicate Identifier Declaration:**
   - **Identifier:** `touchJoystickActive`
   - **Locations:** Declared with `let` at line 1466 and line 3087.
   - **Impact:** Threw `SyntaxError: Identifier 'touchJoystickActive' has already been declared` in modern V8/Chromium, halting script execution.
   - **Resolution:** Retained single declaration at top-level state scope and removed duplicate `let` keyword.

2. **Unused / Obsolete CSS Rules:**
   - `.brand-header`: Legacy header container from initial prototype before Left to Survive military tactical top-bar.
   - `.brand-title` & `.brand-subtitle`: Redundant text styles superseded by `#main-menu-overlay` typography.
   - `.avatar-container` & `.ethan-avatar`: Deprecated 2D portrait elements superseded by 3D Orbit Hero Showcase.
   - `.offline`: Deprecated network indicator replaced by dynamic `#connection-badge`.
   - **Action Taken:** Removed 48 lines of dead CSS from `index.html`, `docs/index.html`, and `LocalPlayable/index.html`.

3. **Missing DOM References with Unchecked Calls:**
   - `btnFire.addEventListener(...)` at line 3367 attempted to bind listeners to `document.getElementById('btn-fire')` when the element was missing from the DOM, throwing `TypeError: Cannot read properties of null`.
   - **Resolution:** Restored action button DOM markup and added defensive null checks to all element queries.

---

### 3. Backend & C# Analysis
- **C# Unused Usings:** Audited across 74 scripts. Zero unused namespaces detected.
- **Unreachable Code:** Zero unreachable code blocks detected.
- **Obsolete APIs:** All Unity 6 Input and Rendering calls use modern URP 17.x and Input System APIs.
