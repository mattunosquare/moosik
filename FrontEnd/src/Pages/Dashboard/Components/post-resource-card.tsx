import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { Grid } from "@mui/material";
import { Card } from "@mui/material";
import CardContent from "@mui/material/CardContent";
import { IPostDetails } from "../../../Common/Interfaces";

interface IProps {
  post: IPostDetails;
}

const PostResourceCard = ({ post }: IProps) => {
  const postResource = post.postResources[0];

  if (postResource == null) {
    return (
      <div>
        <Box>
          <Grid
            container
            direction="column"
            justifyContent="space-around"
            style={{ height: "100%" }}
          >
            <Grid item>
              <Card variant="elevation">
                <CardContent>
                  <Typography variant="subtitle2">No Post Resource</Typography>
                </CardContent>
              </Card>
            </Grid>
          </Grid>
        </Box>
      </div>
    );
  }

  return (
    <div>
      <Box>
        <Grid
          container
          direction="column"
          justifyContent="space-around"
          style={{ height: "100%" }}
        >
          <Grid item>
            <Card variant="elevation">
              <CardContent>
                <Typography variant="subtitle1">
                  Post Resource - Type: {postResource.resourceType.description}
                </Typography>
                <Typography variant="body1">
                  Title: {postResource.title}
                </Typography>
                <Grid container justifyContent="flex-start">
                  <Typography variant="body1">
                    Value: {postResource.value}
                  </Typography>
                </Grid>
              </CardContent>
            </Card>
          </Grid>
        </Grid>
      </Box>
    </div>
  );
};

export default PostResourceCard;
