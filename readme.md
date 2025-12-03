# 🎮 Link To The Past - Unity Game

<p align="center">
  <img src="https://img.shields.io/badge/Unity-6000.2.8f1-black?style=for-the-badge&logo=unity" alt="Unity Version">
  <img src="https://img.shields.io/badge/Platform-Windows%20%7C%20macOS-blue?style=for-the-badge" alt="Platform">
  <img src="https://img.shields.io/badge/Genre-Action%20RPG-green?style=for-the-badge" alt="Genre">
  <img src="https://img.shields.io/badge/Status-In%20Development-orange?style=for-the-badge" alt="Status">
</p>

---

## 📖 Descripción

**Link To The Past** es un juego de acción y aventura 2D inspirado en los clásicos RPG de exploración. El jugador controla a **Mira**, quien debe superar múltiples pruebas, derrotar enemigos y recolectar objetos para avanzar a través de diferentes escenas y desbloquear nuevas áreas.

### 🎯 Características Principales

- 🗡️ **Sistema de combate** con disparo direccional
- 🎒 **Sistema de inventario** con recolección de objetos
- ⚗️ **Sistema de crafteo** para crear llaves y herramientas
- 🚪 **Puertas desbloqueables** basadas en progreso y objetos
- 👾 **Enemigos con IA** que persiguen, mantienen distancia y disparan
- 💾 **Persistencia de progreso** entre escenas
- ❤️ **Sistema de vida** para jugador y enemigos

---

## 🎮 Controles

| Tecla | Acción |
|-------|--------|
| `W` `A` `S` `D` / Flechas | Movimiento |
| `X` | Interactuar (puertas, cofres) |
| `C` | Disparar/Atacar |
| `Tab` | Abrir panel de crafteo |
| `E` | Recoger objetos |

---

## 🗺️ Escenas del Juego

| Escena | Descripción |
|--------|-------------|
| **INICIO** | Menú principal |
| **SampleScene** | Área central del juego |
| **Taller** | Taller de forja para obtener armas |
| **PRUEBA 1** | Primera prueba de combate |
| **PRUEBA 2** | Segunda prueba de combate |
| **ZonaFinal** | Área final del juego |

---

## 🛠️ Tecnologías Utilizadas

### Motor de Juego
- **Unity 6000.2.8f1** (Unity 6 LTS)

### Lenguaje de Programación
- **C#** (.NET)

### Paquetes y Dependencias

| Paquete | Versión | Uso |
|---------|---------|-----|
| Universal Render Pipeline (URP) | 17.2.0 | Renderizado 2D optimizado |
| Input System | 1.14.2 | Sistema de entrada moderno |
| TextMesh Pro | (incluido) | UI y texto de alta calidad |
| 2D Feature Set | 2.0.1 | Herramientas para desarrollo 2D |
| Timeline | 1.8.9 | Secuencias y animaciones |
| AI Navigation | 2.0.9 | Navegación de enemigos |

### Herramientas de Desarrollo
- **Visual Studio Code** / **Visual Studio** - IDE
- **Git** - Control de versiones
- **GitHub** - Repositorio remoto

---

## 📁 Estructura del Proyecto

```
Project Link To The Past/
├── Assets/
│   ├── Prefabs/           # Prefabs del juego
│   ├── Scenes/            # Escenas del juego
│   ├── Scripts/           # Scripts C#
│   │   ├── MiraController.cs
│   │   ├── PlayerCombat.cs
│   │   ├── PlayerHealth.cs
│   │   ├── EnemyController.cs
│   │   ├── EnemyBullet.cs
│   │   ├── Bullet.cs
│   │   ├── InventorySystem.cs
│   │   ├── CraftingSystem.cs
│   │   ├── DoorInteraction.cs
│   │   ├── GameProgress.cs
│   │   └── ...
│   ├── Settings/          # Configuración de render
│   ├── TextMesh Pro/      # Assets de texto
│   └── Tiles/             # Tiles para el mapa
├── Packages/              # Dependencias Unity
├── ProjectSettings/       # Configuración del proyecto
└── README.md
```

---

## 🎯 Sistemas del Juego

### Sistema de Combate
- Disparo en **dirección opuesta** al movimiento
- Balas con **detección de radio** para mayor precisión
- Enemigos con vida y sistema de daño

### Sistema de Enemigos
- **IA de persecución** con detección de rango
- **Separación automática** para evitar agrupamiento
- **Disparo hacia el jugador** con cooldown
- **Mantenimiento de distancia** óptima

### Sistema de Progreso
- **Persistencia de pruebas completadas**
- **Enemigos derrotados** no reaparecen
- **Objetos recolectados** permanecen recogidos
- **Puertas desbloqueables** según progreso

---

## 🚀 Instalación y Ejecución

### Requisitos
- Unity Hub
- Unity 6000.2.8f1 o superior

### Pasos
1. Clonar el repositorio:
   ```bash
   git clone https://github.com/phol232/Legend_Zelda.git
   ```
2. Abrir Unity Hub
3. Añadir el proyecto desde la carpeta clonada
4. Abrir el proyecto con Unity 6000.2.8f1
5. Abrir la escena `INICIO` en `Assets/Scenes/`
6. Presionar **Play** ▶️

---

## 📸 Capturas de Pantalla

<p align="center">
  <img src="Screenshots/gameplay.png" alt="Gameplay Screenshot" width="800">
</p>

<p align="center">
  <em>Escena del Taller - El jugador explorando el área de forja</em>
</p>

---

## 🔮 Futuras Mejoras

- [ ] Sistema de guardado permanente
- [ ] Más tipos de enemigos
- [ ] Power-ups y mejoras
- [ ] Sistema de diálogos
- [ ] Efectos de sonido y música
- [ ] Animaciones de personajes
- [ ] Boss final

---

## 👨‍💻 Desarrollador

<p align="center">
  <strong>Phol Edwin Taquiri Rojas</strong>
</p>

<p align="center">
  <a href="https://github.com/phol232">
    <img src="https://img.shields.io/badge/GitHub-phol232-181717?style=for-the-badge&logo=github" alt="GitHub">
  </a>
</p>

---

## 📄 Licencia

Este proyecto es de uso educativo y personal.

---

## 🙏 Agradecimientos

- Inspirado en **The Legend of Zelda: A Link to the Past**
- Sprites y assets de la comunidad de desarrollo indie
- Unity Technologies por el motor de juego

---

<p align="center">
  Desarrollado con ❤️ y ☕ por <strong>Phol Edwin Taquiri Rojas</strong>
</p>

<p align="center">
  <em>Diciembre 2025</em>
</p>
