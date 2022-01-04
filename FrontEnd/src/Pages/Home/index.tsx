import { Grid, makeStyles, Theme } from "@mui/material";
import MakeARecommendation from "../../Components/make-recommendation";
import SearchAutoComplete from "../../Components/search-bar";
import MakeARequest from "./Components/make-request";
import { styled } from "@mui/system";
import { Typography } from "@mui/material";
import { TopLevelGridContainer } from "./styled";

function Recommendations() {
  return (
    <TopLevelGridContainer>
      <Grid
        sx={{ height: "85vh" }}
        container
        direction="column"
        alignItems="center"
        justifyContent="space-between"
      >
        <Grid
          item
          sx={{
            alignSelf: { xs: "center", md: "flex-end" },
          }}
        >
          <MakeARecommendation />
        </Grid>

        <Grid item style={{ width: "80%" }}>
          <Typography variant="h6">
            Let us know what you're up to, and we'll find some music to fit :)
          </Typography>
          <SearchAutoComplete />
        </Grid>

        <Grid item>
          <MakeARequest />
        </Grid>
      </Grid>
    </TopLevelGridContainer>
  );
}
export default Recommendations;
