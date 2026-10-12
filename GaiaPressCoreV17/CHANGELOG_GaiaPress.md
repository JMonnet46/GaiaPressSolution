# **Journal des modifications - Base de données GaiaPress**

---
**Dernière mise à jour** : 19/06/2026
**Auteur principal** : Jacques Monnet

---

## **📅 Historique des modifications**

---
### **🔹 19/06/2026 - Augmentation de la précision des colonnes décimales**
**Auteur** : Jacques Monnet
**Contexte** : Besoin d'augmenter la précision des colonnes pour les calculs géotechniques.

#### **📌 Modifications apportées**
   **Table**                  | **Colonne**       | **Ancien Type** | **Nouveau Type** | **Raison**                          |
 |----------------------------|-------------------|-----------------|------------------|-------------------------------------|
 | `LesEssaisExpansionPropre` | `CoefficientA`    | `DECIMAL(10,4)` | `DECIMAL(10,6)` | Précision accrue pour les calculs. |
 | `LesEssaisExpansionPropre` | `CoefficientB`    | `DECIMAL(10,4)` | `DECIMAL(10,6)` | Précision accrue pour les calculs. |
 | `LesMesuresForage`         | `deformation`     | `DECIMAL(10,4)` | `DECIMAL(10,6)` | Précision accrue pour les mesures.  |

#### **📝 Scripts SQL exécutés**
```sql
ALTER TABLE dbo.LesEssaisExpansionPropre ALTER COLUMN CoefficientA DECIMAL(10, 6);
ALTER TABLE dbo.LesEssaisExpansionPropre ALTER COLUMN CoefficientB DECIMAL(10, 6);
ALTER TABLE dbo.LesMesuresForage ALTER COLUMN deformation DECIMAL(10, 6);
