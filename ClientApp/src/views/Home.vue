<template>
  <div>
    <!--
        <div class="hello">
            <h1>{{ msg }}</h1>
            -->
    <header>
      <nav>
        <div class="nav-wrapper">
          <a class="brand-logo" href="./"><i class="material-icons">home</i></a>
          <ul id="nav-mobile">
            <li>
              <a href="#">About<i class="material-icons">info</i></a>
            </li>
            <li>
              <a href="#"
                >Join the game<i class="material-icons">sports_esports</i></a
              >
            </li>
            <li>
              <a href="#">Teacher zone<i class="material-icons">work</i></a>
            </li>
            <li>
              <a href="#">Register<i class="material-icons">person_add</i></a>
            </li>
          </ul>
          <a class="sidenav-trigger" data-target="slide-out" href="#"
            ><i class="material-icons">menu</i></a
          >
        </div>
      </nav>
      <ul class="sidenav" id="slide-out">
        <li>
          <a href="#">About<i class="material-icons">info</i></a>
        </li>
        <li>
          <a href="#"
            >Join the game<i class="material-icons">sports_esports</i></a
          >
        </li>
        <li>
          <a href="#">Teacher zone<i class="material-icons">work</i></a>
        </li>
        <li>
          <a href="#">Register<i class="material-icons">person_add</i></a>
        </li>
      </ul>
    </header>

    <main>
      <div class="container">
        <div class="carousel carousel-slider">
          <a class="carousel-item" href="#">
            <div class="imageContainer">
              <img
                alt="Screenshot from game displaying maze"
                src="../assets/test.png"
                style="height: 100%; width: 100%; object-fit: contain"
              />
              <div class="overImage">
                Simple yet attractive educational game
              </div>
            </div>
          </a>
          <a class="carousel-item" href="#">
            <div class="imageContainer">
              <img
                alt="Student's panel view"
                src="../assets/test.png"
                style="height: 100%; width: 100%; object-fit: contain"
              />
              <div class="overImage">Students can choose from many topics</div>
            </div>
          </a>
          <a class="carousel-item" href="#">
            <div class="imageContainer">
              <img
                alt="Teacher's panel view"
                src="../assets/test.png"
                style="height: 100%; width: 100%; object-fit: contain"
              />
              <div class="overImage">
                Teachers can share their scenarios with student
              </div>
            </div>
          </a>
          <a class="carousel-item" href="#">
            <div class="imageContainer">
              <img
                alt="Statistics"
                src="../assets/test.png"
                style="height: 100%; width: 100%; object-fit: contain"
              />
              <div class="overImage">
                Teachers can analyze students' progress
              </div>
            </div>
          </a>
        </div>
      </div>

      <div class="container">
        <h3>Benefits for students</h3>
        <div class="row">
          <div class="col s12 m6 l4">
            <div class="card">
              <div class="card-content">
                <span class="card-title"
                  ><i class="large material-icons">sports_esports</i
                  ><br />Involving learning</span
                >
                <p>
                  I am a very simple card. I am good at containing small bits of
                  information. I am convenient because I require little markup
                  to use effectively.
                </p>
              </div>
            </div>
          </div>
          <div class="col s12 m6 l4">
            <div class="card">
              <div class="card-content">
                <span class="card-title"
                  ><i class="large material-icons">laptop</i><br />Modern
                  testing</span
                >
                <p>
                  I am a very simple card. I am good at containing small bits of
                  information. I am convenient because I require little markup
                  to use effectively.
                </p>
              </div>
            </div>
          </div>
          <div class="col s12 m6 l4">
            <div class="card">
              <div class="card-content">
                <span class="card-title"
                  ><i class="large material-icons">format_list_numbered_rtl</i
                  ><br />Competing</span
                >
                <p>
                  I am a very simple card. I am good at containing small bits of
                  information. I am convenient because I require little markup
                  to use effectively.
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="container">
        <h3>Benefits for teachers</h3>
        <div class="row">
          <div class="col s12 m6 l3">
            <div class="card">
              <div class="card-content">
                <span class="card-title">
                  <i class="large material-icons">grade</i><br />Students'
                  evaluation
                </span>
                <p>
                  I am a very simple card. I am good at containing small bits of
                  information. I am convenient because I require little markup
                  to use effectively.
                </p>
              </div>
            </div>
          </div>
          <div class="col s12 m6 l3">
            <div class="card">
              <div class="card-content">
                <span class="card-title"
                  ><i class="large material-icons">replay</i><br />Reusable
                  scenarios</span
                >
                <p>
                  I am a very simple card. I am good at containing small bits of
                  information. I am convenient because I require little markup
                  to use effectively.
                </p>
              </div>
            </div>
          </div>
          <div class="col s12 m6 l3">
            <div class="card">
              <div class="card-content">
                <span class="card-title"
                  ><i class="large material-icons">attractions</i
                  ><br />Student-attractive form</span
                >
                <p>
                  I am a very simple card. I am good at containing small bits of
                  information. I am convenient because I require little markup
                  to use effectively.
                </p>
              </div>
            </div>
          </div>
          <div class="col s12 m6 l3">
            <div class="card">
              <div class="card-content">
                <span class="card-title">
                  <i class="large material-icons">quiz</i><br />Alternative for
                  tests
                </span>
                <p>
                  I am a very simple card. I am good at containing small bits of
                  information. I am convenient because I require little markup
                  to use effectively.
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="container">
        <div class="auto-resizable-iframe">
          <div>
            <iframe
              allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
              allowfullscreen
              frameborder="0"
              src="https://www.youtube.com/embed/fvtxgkC-oz8"
              title="YouTube video player"
            ></iframe>
          </div>
        </div>
      </div>
    </main>

    <footer>
      <div class="footer-copyright grey darken-4 white-text">� 2021</div>
    </footer>
  </div>
</template>

<script>
import M from "materialize-css";

export default {
  mounted() {
    // Initialize carousel
    M.Carousel.init(document.querySelectorAll(".carousel"), {
      fullWidth: true,
      duration: 500,
    });

    // Initialize side navigation
    M.Sidenav.init(document.querySelectorAll(".sidenav"), {
      edge: "right",
    });

    //Move carousel each 5s
    const carousel = M.Carousel.getInstance(
      document.querySelectorAll(".carousel")[0]
    );
    setInterval(function () {
      carousel.next();
    }, 7000);
  },
};
</script>

<style scoped>
@import "../css/custom/carousel_additions.css";
@import "../css/custom/video_resizable_container.css";
</style>
<!--<script>
export default {
  name: 'Home',
  props: {
    msg: String
  }
}
</script>-->

<!-- Add "scoped" attribute to limit CSS to this component only -->
<!--<style scoped>
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
-->