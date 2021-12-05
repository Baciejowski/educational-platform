<template>
  <div>
    <main>
      <div class="container">
        <div class="carousel carousel-slider">
          <a class="carousel-item" href="#">
            <div class="imageContainer">
              <img
                alt="Screenshot from game displaying maze"
                src="../assets/1panel.png"
                style="height: 100%; width: 100%; object-fit: contain"
              />
              <div class="overImage">
                Simple yet attractive educational game
              </div>
            </div>
          </a>
          <!--<a class="carousel-item" href="#">
            <div class="imageContainer">
              <img
                alt="Student's panel view"
                src="../assets/test.png"
                style="height: 100%; width: 100%; object-fit: contain"
              />
              <div class="overImage">Students can choose from many topics</div>
            </div>
          </a>-->
          <a class="carousel-item" href="#">
            <div class="imageContainer">
              <img
                alt="Teacher's panel view"
                src="../assets/3panel.png"
                style="height: 100%; width: 100%; object-fit: contain"
              />
              <div class="overImage">
                Teachers can share their scenarios with student
              </div>
            </div>
          </a>
          <!--<a class="carousel-item" href="#">
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
          </a>-->
        </div>
      </div>

      <div class="container" style="margin-top:20px">
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
                  No more boring homeworks! <br />Here you can have fun while still learning and revising your knowledge before lesson or test. Your grades can improve!
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
                  Your knowledge can be tested in more interesting way. Not everything is important so why not to skip some questions? AI will decide if a question makes sense to be asked.
                </p>
              </div>
            </div>
          </div>
          <div class="col s12 m6 l4">
            <div class="card">
              <div class="card-content">
                <span class="card-title">
                  <i class="large material-icons">format_list_numbered_rtl</i>
                  <br />Competing</span>
                <p>
                  Students can compare their statistics like money or experience points obtained during game or who finished the maze as the first one.
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
                  Students results can be taken into account as activity or bonus points during grading process.
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
                  You can copy scenarios, modify wuestions and use them again with different group of studnets.
                </p>
              </div>
            </div>
          </div>
          <div class="col s12 m6 l3">
            <div class="card">
              <div class="card-content">
                <span class="card-title"
                  ><i class="large material-icons">attractions</i
                  ><br />Student-attractive</span
                >
                <p>
                    Make the lessons more attractive! Students learn more when they are active and like what they do.
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
                  You don't have to check paper quizes anymore! Students will revise knowledge by plaing game.
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
        <br/>
      <!--<div class="container">
        <img src="../assets/Gif.gif" style="height: 100%; width: 100%; object-fit: contain"/>-->
        <!--<div class="auto-resizable-iframe">
          <div>
            <iframe
              allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
              allowfullscreen
              frameborder="0"
              src="https://www.youtube.com/embed/fvtxgkC-oz8"
              title="YouTube video player"
            ></iframe>
          </div>
        </div>-->
      <!--</div>-->
    </main>

    <footer>
      <div class="footer-copyright grey darken-4 white-text">2021</div>
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
    }, 3000);
  },
};
</script>

<style scoped>
@import "../css/custom/carousel_additions.css";
@import "../css/custom/video_resizable_container.css";
p {
    text-align: center;
}
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